﻿CREATE VIEW VW_ESTATISTICAS_FINAL
AS
SELECT DADOS.*
FROM	(

SELECT	LINHA_ATUAL.IDT_CONSULTA,
		CASE WHEN (LINHA_ATUAL.MEDIA_COMPRA < LINHA_ANTERIOR.MEDIA_COMPRA) THEN 'DESCENDO' ELSE 'SUBINDO' END TENDENCIA_MEDIA_COMPRA,
		CASE WHEN (LINHA_ATUAL.MINIMA_COMPRA < LINHA_ANTERIOR.MINIMA_COMPRA) THEN 'DESCENDO' ELSE 'SUBINDO' END TENDENCIA_MINIMA_COMPRA,
		CASE WHEN (LINHA_ATUAL.MEDIA_VENDA < LINHA_ANTERIOR.MEDIA_VENDA) THEN 'DESCENDO' ELSE 'SUBINDO' END TENDENCIA_MEDIA_VENDA,
		CASE WHEN (LINHA_ATUAL.MAX_VENDA < LINHA_ANTERIOR.MAX_VENDA) THEN 'DESCENDO' ELSE 'SUBINDO' END TENDENCIA_MAX_VENDA,
		CASE WHEN (LINHA_ATUAL.MEDIA_QTD_COMPRA < LINHA_ANTERIOR.MEDIA_QTD_COMPRA) THEN 'DESCENDO' ELSE 'SUBINDO' END TENDENCIA_QTD_COMPRA,
		CASE WHEN (LINHA_ATUAL.MEDIA_QTD_VENDA < LINHA_ANTERIOR.MEDIA_QTD_VENDA) THEN 'DESCENDO' ELSE 'SUBINDO' END TENDENCIA_QTD_VENDA,
		DADOS_ABERTOS.vlr_compra - DADOS_ABERTOS.vlr_venda DIF_COMPRA_X_VENDA,
		(DADOS_ABERTOS.vlr_compra-DADOS_ABERTOS.vlr_minimo)/ (DADOS_ABERTOS.vlr_maximo-DADOS_ABERTOS.vlr_minimo) DISTANCIA_MAXIMO,
		LINHA_ATUAL.MEDIA_VLR_VENDA,
		DADOS_ABERTOS.QTD_VENDA_LIVRO,
		DADOS_ABERTOS.QTD_COMPRA_LIVRO		
FROM	(
		SELECT	ATUAL.idt_consulta,
				AVG(GRUPO.EVOL_MEDIO_COMPRA_LIVRO) MEDIA_COMPRA,
				AVG(GRUPO.EVOL_MIN_COMPRA_LIVRO) MINIMA_COMPRA,
				AVG(GRUPO.EVOL_MEDIO_VENDA_LIVRO) MEDIA_VENDA,
				AVG(GRUPO.EVOL_MAX_VENDA_LIVRO) MAX_VENDA,
				AVG(GRUPO.EVOL_VLR_VENDA) MEDIA_VLR_VENDA,
				AVG(GRUPO.EVOL_QTD_COMPRA_LIVRO) MEDIA_QTD_COMPRA,
				AVG(GRUPO.EVOL_QTD_VENDA_LIVRO) MEDIA_QTD_VENDA
		FROM	VW_ESTATISTICAS_3_CONSULTA_LIVRO ATUAL LEFT JOIN VW_ESTATISTICAS_3_CONSULTA_LIVRO GRUPO
					ON(	GRUPO.idt_consulta BETWEEN ATUAL.idt_consulta-(ATUAL.qtd_registros_anterior-1) AND ATUAL.idt_consulta)
		GROUP BY ATUAL.idt_consulta
		) LINHA_ATUAL
LEFT JOIN 
		(
		SELECT	ATUAL.idt_consulta,
				AVG(GRUPO.EVOL_MEDIO_COMPRA_LIVRO) MEDIA_COMPRA,
				AVG(GRUPO.EVOL_MIN_COMPRA_LIVRO) MINIMA_COMPRA,
				AVG(GRUPO.EVOL_MEDIO_VENDA_LIVRO) MEDIA_VENDA,
				AVG(GRUPO.EVOL_MAX_VENDA_LIVRO) MAX_VENDA,
				AVG(GRUPO.EVOL_VLR_VENDA) MEDIA_VLR_VENDA,
				AVG(GRUPO.EVOL_QTD_COMPRA_LIVRO) MEDIA_QTD_COMPRA,
				AVG(GRUPO.EVOL_QTD_VENDA_LIVRO) MEDIA_QTD_VENDA
		FROM	VW_ESTATISTICAS_3_CONSULTA_LIVRO ATUAL LEFT JOIN VW_ESTATISTICAS_3_CONSULTA_LIVRO GRUPO
					ON(	GRUPO.idt_consulta BETWEEN ATUAL.idt_consulta-(ATUAL.qtd_registros_anterior-1) AND ATUAL.idt_consulta)
		GROUP BY ATUAL.idt_consulta
		) LINHA_ANTERIOR
			ON(	LINHA_ANTERIOR.idt_consulta = LINHA_ATUAL.idt_consulta-1)
JOIN	VW_ESTATISTICAS_2_CONSULTA DADOS_ABERTOS
			ON(	DADOS_ABERTOS.idt_consulta = LINHA_ATUAL.idt_consulta)
)DADOS 


