
-------------------------------------------------------------------------------

create database uaifuel
go
use uaifuel
go


-- veiculo	---------------------------------------------------------------

create table veiculos
(	
	placa			char(7)			not null,
	cor				char(20)		not null,	
	status			int				not	null,
	modelo			varchar(20)		not null,
	ano_fabricacao	int				not null,
	ano_modelo		int				not null

	constraint	pk_veiculo	primary key (placa),
	constraint	ak_veiculo	unique(placa)
)
go

-- estados	------------------------------------------------------------------

create table estados 
( 
  uf	char(2)		not null,
  nome	varchar(75) not null,

  constraint pk_estado	primary key (uf),
  constraint ak_estado	unique (uf)
) 
go

-- cidades	------------------------------------------------------------------

create table cidades
(
  id			int				not null	identity,
  nome			varchar(120)	not null,
  estado_uf		char(2)			not null,
  
  constraint pk_cidade			primary key(id),
  constraint fk_cidade__estado	foreign key(estado_uf) references estados
) 
go


-- pessoas	-------------------------------------------------------------------

create table pessoas
(
	id		int				not null	identity,
	nome	varchar(100)	not null,
	login	varchar(100)	not null,
	senha	varchar(100)	not null,
	status	int				not null,

	constraint	pk_pessoa		primary key	(id),
	constraint	ak_pessoa_login	unique(login)
)
go


-- postos	-------------------------------------------------------------------

create table postos
(	
	pessoa_id	int				not null,
	cnpj		varchar(20)		not null,
	telefone	varchar(30)		not null,	

	constraint	pk_posto			primary key(pessoa_id),
	constraint	fk_posto__pessoa	foreign key(pessoa_id) references pessoas,
	constraint	ak_posto_cnpj		unique(cnpj)
)
go

-- motoristas	---------------------------------------------------------------

create table motoristas
(	
	pessoa_id	int				not null,
	cpf			varchar(15)		not null,
	credito		money			not null default 0,

	constraint	pk_motorista			primary key(pessoa_id),
	constraint	fk_motorista__pessoa	foreign key(pessoa_id) references pessoas,
	constraint	ak_motorista_cpf		unique(cpf)
)
go

-- administrador	-----------------------------------------------------------

create table administradores
(	
	pessoa_id	int				not null,

	constraint	pk_administrador			primary key(pessoa_id),	
	constraint	fk_administrador__pessoa	foreign key(pessoa_id) references pessoas
)
go


-- emails	-------------------------------------------------------------------

create table emails
(
	pessoa_id	int				not null, 
	endereco	varchar(50)		not null,

	constraint	pk_email			primary key	(pessoa_id, endereco),
	constraint	fk_email__pessoa	foreign key	(pessoa_id			) references pessoas	
)
go


-- enderecos	---------------------------------------------------------------


create table enderecos
(
	posto_id	int				not null, 
	id			int				not null	identity,
	cep			char(9)			not	null,
	localidade	varchar(200)	not null,
	cidade_id	int				not null

	constraint	pk_endereco			primary key	(posto_id, id),
	constraint	fk_endereco__posto	foreign key	(posto_id 	 ) references postos,
	constraint	fk_endereco__cidade	foreign key	(cidade_id 	 ) references cidades
)
go


-- combustiveis	---------------------------------------------------------------

create table combustiveis
(
	posto_id	int				not null, 
	id			int				not null	identity,
	nome		varchar(50)		not null,
	status		int				not	null	default 1,

	constraint	pk_combustiveis			primary key	(posto_id, id),
	constraint	fk_combustiveis__posto	foreign key	(posto_id 	 ) references postos
)
go

-- abastecimentos	-----------------------------------------------------------

create table abastecimentos 
(
	id				int			not null	identity,
	data			datetime	not null,
	cupom_fiscal	varchar(50)		null,
	valor_total		money			null	default 0,	
	status			int			not null,
	veiculo_placa	char(7)		not null,
	posto_id		int			not null,
	motorista_id	int			not null,

	constraint	pk_abastecimento				primary key (id),
	constraint	fk_abastecimento__veiculo		foreign key (veiculo_placa) references veiculos,
	constraint	fk_abastecimento__posto			foreign key (posto_id) references postos,
	constraint	fk_abastecimento__motorista		foreign key (motorista_id) references motoristas,
	constraint	ak_abastecimento_cupom_fiscal	unique		(cupom_fiscal),
	constraint	ck_abastecimento_valor_total	check		(valor_total >= 0),
	constraint	ck_abastecimento_status			check		(status in (1,2,3))	
)
go


-- abastecimentos__combustiveis	-----------------------------------------------

create table abastecimentos__combustiveis
(
	abastecimento_id	int		not null,
	posto_id			int		not null,
	combustivel_id		int		not null,
	valor				money	not null,
	litros				decimal	not null

	constraint	pk_abastecimento_combustivel	primary key(abastecimento_id, posto_id, combustivel_id),
	constraint	fk_ac__abastecimento			foreign key(abastecimento_id) references abastecimentos,
	constraint	fk_ac__posto					foreign key(posto_id 	    ) references postos,
	constraint	fk_ac__combustivel				foreign key(posto_id, combustivel_id) references combustiveis
)
go


--===============================================================================
-- VIEW'S	---------------------------------------------------------------------

create view v_motoristas
as
select	p.id as MotoristaId, p.nome as MotoristaNome 
from	pessoas as p inner join motoristas as m
 on p.id = m.pessoa_id
go

create view v_postos
as
select	p.id as PostoId, p.nome as PostoNome
from	pessoas as p inner join postos as pt
on	p.id = pt.pessoa_id
go

create view v_abastecimentos
as
select a.id as Id, a.data as Data, a.cupom_fiscal as CupomFiscal, a.valor_total as ValorTotal, a.status as Status, 
		a.veiculo_placa as VeiculoPlaca,  po.PostoId, po.PostoNome, m.MotoristaId, m.MotoristaNome
from abastecimentos a 
	inner join v_postos po	on a.posto_id = po.PostoId
	inner join v_motoristas m on a.motorista_id = m.MotoristaId
	inner join veiculos v	on a.veiculo_placa = v.placa
go


--===============================================================================
-- STORED PROCEDURE'S	---------------------------------------------------------

CREATE OR ALTER  PROCEDURE procAbastecimento
@v_acao int = null,
@v_data datetime = null, 
@v_cupomFiscal varchar(50) = null, 
@v_valorTotal money = null, 
@v_status int = null,
@v_veiculoPlaca varchar(7) = null, 
@v_motoristaId int = null, 
@v_postoId int = null,

@v_combustivelId int = null,
@v_valor money = null,
@v_litros decimal = null,
@v_abastecimentoId int = null
AS
BEGIN
	IF @v_acao = 1
		BEGIN
			
			insert into abastecimentos 
			(data, cupom_fiscal, valor_total, status, veiculo_placa, motorista_id, posto_id)
             values	
			(CONVERT(DATETIME, @v_data), @v_cupomFiscal, @v_valorTotal, @v_status, 
				@v_veiculoPlaca, @v_motoristaId, @v_postoId);

			SET @v_abastecimentoId = (select @@IDENTITY);

			insert into abastecimentos__combustiveis 
			(abastecimento_id, posto_id, combustivel_id, valor, litros)
            values	
			(@v_abastecimentoId, @v_postoId, @v_combustivelId, @v_valor, @v_litros);

			SELECT @v_abastecimentoId AS AbastecimentoId;
		END;

	IF @v_acao = 2
		BEGIN
			insert into abastecimentos__combustiveis 
			(abastecimento_id, posto_id, combustivel_id, valor, litros)
            values	
			(@v_abastecimentoId, @v_postoId, @v_combustivelId, @v_valor, @v_litros);
		END;
		
	IF @v_acao = 3
		BEGIN
			delete from abastecimentos__combustiveis
            where abastecimento_id = @v_abastecimentoId 
            and posto_id = @v_postoId;

			delete from abastecimentos where id = @v_abastecimentoId
		END;
END
GO

