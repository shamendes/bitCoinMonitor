﻿CREATE VIEW VW_ESTATISTICAS_2_CONSULTA
AS
SELECT	CON.idt_consulta,
		CON.dat_consulta,
		CON.vlr_maximo,
		CON.vlr_minimo,
		CON.vlr_compra,
		CON.vlr_venda,
		CON.vlr_volume,
		CON.vlr_ultimo,
		LVR_COMPRA.QTD_DISPONIVEL QTD_COMPRA_LIVRO,
		LVR_COMPRA.VLR_MINIMO MINIMO_COMPRA_LIVRO,
		LVR_COMPRA.VLR_MEDIO MEDIO_COMPRA_LIVRO,
		LVR_VENDA.QTD_DISPONIVEL QTD_VENDA_LIVRO,
		LVR_VENDA.VLR_MAXIMO MAXIMO_VENDA_LIVRO,
		LVR_VENDA.VLR_MEDIO MEDIO_VENDA_LIVRO
FROM	TB_CONSULTA CON INNER JOIN VW_ESTATISTICAS_1_LIVRO LVR_COMPRA
			ON(	LVR_COMPRA.idt_consulta = CON.idt_consulta AND
				LVR_COMPRA.cod_tipo_ordem = 'C')
JOIN	VW_ESTATISTICAS_1_LIVRO LVR_VENDA
			ON(	LVR_VENDA.idt_consulta = CON.idt_consulta AND
				LVR_VENDA.cod_tipo_ordem = 'V')
