﻿DECLARE @IDT_CONSULTA INT
DECLARE @MRC_OPERAR CHAR(1)
DECLARE @VLR_COMPRA DECIMAL(18,5)
DECLARE @VLR_VENDA DECIMAL(18,5)
DECLARE @VLR_MAXIMO DECIMAL(18,5)
DECLARE @VLR_MINIMO DECIMAL(18,5)

DECLARE @VLR_DIF_MAX_MIN DECIMAL(18,5)
DECLARE @VLR_CARTEIRA DECIMAL(18,5)
DECLARE @VLR_INVESTIDO DECIMAL(18,5)
DECLARE @QTD_BITCOINS DECIMAL(18,5)
DECLARE @PCT_TAXA DECIMAL(18,5)
DECLARE @PCT_RENDIMENTO DECIMAL(18,5)
DECLARE @VLR_SIMULADO_VENDA DECIMAL(18,5)
DECLARE @VLR_MIN_COMPRA DECIMAL(18,5)
DECLARE @VLR_VENDA_BASE DECIMAL(18,5)
DECLARE @FLAG VARCHAR(10)
DECLARE @INDICE DECIMAL(18,5)
DECLARE @IDT_CONSULTA_VENDA INT


SET @VLR_CARTEIRA = 1000
SET @VLR_INVESTIDO = 0
SET @QTD_BITCOINS = 0
SET @PCT_TAXA = 0.003
SET @PCT_RENDIMENTO = 0.02
SET @VLR_SIMULADO_VENDA = 0
SET @VLR_MIN_COMPRA = 0
SET @VLR_VENDA_BASE = 0
SET @FLAG = 'NORMAL'


DECLARE CR CURSOR FOR
SELECT IDT_CONSULTA, vlr_compra, vlr_venda, vlr_maximo, vlr_minimo
FROM TB_CONSULTA


OPEN CR

FETCH CR INTO @IDT_CONSULTA, @VLR_COMPRA,@VLR_VENDA, @VLR_MAXIMO, @VLR_MINIMO

WHILE @@FETCH_STATUS = 0
BEGIN

	IF(@FLAG = 'SUSPENDE') 
	 BEGIN
		
		SELECT	@INDICE = AVG(CASE WHEN C.vlr_compra > C_ANT.vlr_compra THEN 1.00 ELSE -1.00 END)
		FROM	TB_CONSULTA C INNER JOIN TB_CONSULTA C_ANT
					ON(	C_ANT.idt_consulta = C.idt_consulta-1)
		WHERE C.IDT_CONSULTA BETWEEN @IDT_CONSULTA-19 AND @IDT_CONSULTA

		IF( @INDICE > 0)
		 BEGIN
			SELECT	@FLAG = CASE WHEN ( @VLR_COMPRA - MIN(vlr_compra) BETWEEN 1000 AND 2000 ) THEN 'COMPRA' ELSE 'SUSPENDE' END
			FROM	TB_CONSULTA
			WHERE	IDT_CONSULTA BETWEEN   @IDT_CONSULTA_VENDA + 1 AND @IDT_CONSULTA		
		 END
	 END
	
	IF( @FLAG = 'AGUARDAR')
	 BEGIN

		IF(@VLR_VENDA = @VLR_MAXIMO OR @VLR_COMPRA = @VLR_MAXIMO)
		 SET @FLAG = 'NORMAL'
	 END


	IF(@FLAG <> 'SUSPENDE' AND @FLAG <> 'AGUARDAR' ) 
	 BEGIN

	
		--PROCESSO DE COMPRA
		IF(@VLR_CARTEIRA > 0)
		BEGIN

			SET @VLR_DIF_MAX_MIN = @VLR_MAXIMO - @VLR_MINIMO
			SET @PCT_RENDIMENTO = (@VLR_DIF_MAX_MIN/@VLR_MAXIMO)/5
		
			SELECT	@MRC_OPERAR = CASE WHEN (
												DIF_COMPRA_X_VENDA >= TB_PARAMETROS.vlr_dif_compra_x_venda AND
												QTD_COMPRA_LIVRO >  QTD_VENDA_LIVRO AND
												DISTANCIA_MAXIMO < TB_PARAMETROS.pct_ditancia_compra_max AND
												@VLR_DIF_MAX_MIN > TB_PARAMETROS.vlr_dif_max_min OR
												@FLAG = 'COMPRA'
											) THEN 'S' ELSE 'N' END 
			FROM	VW_ESTATISTICAS_FINAL JOIN TB_PARAMETROS 
						ON(1=1)
			WHERE	IDT_CONSULTA = @IDT_CONSULTA

			IF(@MRC_OPERAR = 'S' )
			 BEGIN
			 
					SET @QTD_BITCOINS = (@VLR_CARTEIRA/@VLR_COMPRA) - (@VLR_CARTEIRA/@VLR_COMPRA*@PCT_TAXA)
					SET @VLR_INVESTIDO = @VLR_CARTEIRA
					SET @VLR_CARTEIRA = 0
					SET @VLR_VENDA_BASE = @VLR_VENDA
					IF( @FLAG = 'COMPRA')
					 SET @FLAG = 'AGUARDAR'
					ELSE
					 SET @FLAG = 'NORMAL'
					SELECT 'COMPRA', @IDT_CONSULTA, @VLR_COMPRA

			 END
		 END
		--PROCESSO DE VENDA
		ELSE
		 BEGIN
			SET @VLR_SIMULADO_VENDA = (@QTD_BITCOINS*@VLR_VENDA) - (@QTD_BITCOINS*@VLR_VENDA*@PCT_TAXA)




			SELECT	@MRC_OPERAR = CASE WHEN (
												TENDENCIA_MAX_VENDA = 'DESCENDO' AND
												TENDENCIA_MEDIA_VENDA = 'DESCENDO' AND
												QTD_VENDA_LIVRO >  QTD_COMPRA_LIVRO 
											) THEN 'S' ELSE 'N' END 
			FROM	VW_ESTATISTICAS_FINAL JOIN TB_PARAMETROS 
						ON(1=1)
			WHERE	IDT_CONSULTA = @IDT_CONSULTA


			IF((@MRC_OPERAR = 'S' AND (@VLR_SIMULADO_VENDA/@VLR_INVESTIDO-1 >@PCT_RENDIMENTO)) OR @VLR_VENDA_BASE - @VLR_VENDA > 500) 
			 BEGIN
				IF( @VLR_SIMULADO_VENDA/@VLR_INVESTIDO-1 < 0 ) SET @FLAG = 'SUSPENDE'
			
				SET @VLR_CARTEIRA = @VLR_SIMULADO_VENDA
				SET @QTD_BITCOINS = 0
				SET @VLR_INVESTIDO = 0
				SET @IDT_CONSULTA_VENDA = @IDT_CONSULTA
				SELECT 'VENDA', @IDT_CONSULTA, @VLR_VENDA
			 END
		 END
	 END
	FETCH CR INTO @IDT_CONSULTA,@VLR_COMPRA,@VLR_VENDA, @VLR_MAXIMO, @VLR_MINIMO
  END

  	 SELECT @VLR_CARTEIRA, @QTD_BITCOINS

CLOSE CR
DEALLOCATE CR


