CREATE TABLE [dbo].[TB_CONSULTA] (
    [idt_consulta] INT             IDENTITY (1, 1) NOT NULL,
    [dat_consulta] DATETIME        NOT NULL,
    [vlr_maximo]   DECIMAL (16, 2) NOT NULL,
    [vlr_minimo]   DECIMAL (16, 2) NOT NULL,
    [vlr_ultimo]   DECIMAL (16, 2) NOT NULL,
    [vlr_volume]   DECIMAL (14, 4) NOT NULL,
    [vlr_compra]   DECIMAL (16, 2) NOT NULL,
    [vlr_venda]    DECIMAL (16, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([idt_consulta] ASC)
);




CREATE TABLE [dbo].[TB_LIVRO_ORDENS] (
    [idt_registro]         INT             IDENTITY (1, 1) NOT NULL,
    [idt_consulta]         INT             NOT NULL,
    [cod_tipo_ordem]       CHAR (1)        NOT NULL,
    [idt_ordem]            INT             NOT NULL,
    [mrc_proprietario]     CHAR (1)        NOT NULL,
    [qtd_negociada]        DECIMAL (13, 5) NOT NULL,
    [vlr_preco_limite]     DECIMAL (16, 5) NOT NULL,
    [idt_ultima_negociada] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([idt_registro] ASC),
    CONSTRAINT [FK_TB_CONSULTA_TB_LIVRO] FOREIGN KEY ([idt_consulta]) REFERENCES [dbo].[TB_CONSULTA] ([idt_consulta]),
    CONSTRAINT [CK_TB_LIVRO_COD_TIPO_LIVRO] CHECK ([cod_tipo_ordem]='V' OR [cod_tipo_ordem]='C'),
    CONSTRAINT [CK_TB_LIVRO_MRC_PROPRIETARIO_LIVRO] CHECK ([mrc_proprietario]='N' OR [mrc_proprietario]='S')
);


CREATE TABLE [dbo].[TB_MINHAS_ORDENS] (
    [IDT_ORDEM]           INT             NOT NULL,
    [IDT_TIPO_ORDEM]      CHAR (1)        NOT NULL,
    [IDT_STATUS]          INT             NOT NULL,
    [DAT_CRIACAO]         DATETIME        NOT NULL,
    [DAT_ATUALIZACAO]     DATETIME        NOT NULL,
    [IDT_CONSULTA_ORIGEM] INT             NOT NULL,
    [VLR_LIMITE]          DECIMAL (18, 5) NOT NULL,
    [QTD_MOEDA]           DECIMAL (18, 5) NOT NULL,
    [QTD_EXECUTADA]       DECIMAL (18, 5) NOT NULL,
    [VLR_MEDIO_EXECUTADO] DECIMAL (18, 5) NOT NULL,
    [VLR_TAXA]            DECIMAL (18, 5) NOT NULL,
    [MRC_AGUARDAR]        CHAR (1)        NOT NULL,
    PRIMARY KEY CLUSTERED ([IDT_ORDEM] ASC),
    CONSTRAINT [FK_TB_CONSULTA_TB_MINHAS_ORDENS] FOREIGN KEY ([IDT_CONSULTA_ORIGEM]) REFERENCES [dbo].[TB_CONSULTA] ([idt_consulta])
);




CREATE TABLE [dbo].[TB_OPERACAO] (
    [IDT_ORDEM]    INT             NOT NULL,
    [IDT_OPERACAO] INT             NOT NULL,
    [DAT_OPERACAO] DATETIME        NOT NULL,
    [QTD_MOEDA]    DECIMAL (18, 5) NOT NULL,
    [VLR_OPERACAO] DECIMAL (18, 5) NOT NULL,
    [VLR_TAXA]     DECIMAL (18, 5) NOT NULL,
    PRIMARY KEY CLUSTERED ([IDT_OPERACAO] ASC, [IDT_ORDEM] ASC),
    CONSTRAINT [FK_TB_MINHAS_ORDENS_TB_OPERACAO] FOREIGN KEY ([IDT_ORDEM]) REFERENCES [dbo].[TB_MINHAS_ORDENS] ([IDT_ORDEM])
);




CREATE TABLE [dbo].[TB_CONSULTA_BITSTAMP] (
    [idt_consulta] INT             IDENTITY (1, 1) NOT NULL,
    [dat_consulta] DATETIME        NOT NULL,
    [vlr_maximo]   DECIMAL (16, 2) NOT NULL,
    [vlr_minimo]   DECIMAL (16, 2) NOT NULL,
    [vlr_ultimo]   DECIMAL (16, 2) NOT NULL,
    [vlr_volume]   DECIMAL (14, 4) NOT NULL,
    [vlr_compra]   DECIMAL (16, 2) NOT NULL,
    [vlr_venda]    DECIMAL (16, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([idt_consulta] ASC)
);

