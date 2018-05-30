CREATE VIEW VW_ESTATISTICAS_1_LIVRO
AS
		select	LVR.idt_consulta,
				LVR.cod_tipo_ordem,
				SUM(LVR.qtd_negociada) QTD_DISPONIVEL, 
				MAX(LVR.vlr_preco_limite) VLR_MAXIMO,
				MIN(LVR.vlr_preco_limite) VLR_MINIMO,
				SUM(LVR.qtd_negociada * LVR.vlr_preco_limite) / SUM(LVR.qtd_negociada) VLR_MEDIO
		from	TB_LIVRO_ORDENS LVR
		GROUP BY LVR.idt_consulta,
				LVR.cod_tipo_ordem
