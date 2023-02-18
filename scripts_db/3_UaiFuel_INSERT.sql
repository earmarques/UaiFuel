
-------------------------------------------------------------------------------

use uaifuel
go

-- veiculo	---------------------------------------------------------------

insert into veiculos (placa, cor, status, modelo, ano_fabricacao, ano_modelo) 
		values	('DTU3934', 'CINZA COSMOS', 3, 'GOL', 2007, 2007),
				('FUS1955', 'BRANCO', 1, 'FUSCA', 1955, 1955    ),
				('FUS1974', 'AMARELO', 1, 'FUSCA', 1974, 1974),
				('FIA1147', 'AZUL', 4, 'FIAT', 1981, 1982),
				('CCC1111', 'VERDE', 1, 'SCANIA 4587 BITREM', 2018, 2018),
				('CCC2222', 'BRANCO PÉROLA', 1, 'VOLVO 7854', 2020, 2021),
				('CCC3333', 'AZUL ESCURO', 2, 'VOLVO 7854', 2020, 2021),
				('BRA4C44', 'ROXO', 1, 'IVECO 458 GRANELEIRO', 2021, 2021)
go


-- pessoas	-------------------------------------------------------------------

insert into pessoas (nome, login, senha, status) 
		values	('AUTO POSTO PINATTO',		'pinatto',	'123',	 1),
				('ELIAS MANTOVANI',			'elias',	'123',	 1),
				('POSTO CINQUENTÃO',		'posto50',	'123',	 1),
				('PEDRO COIMBRA',			'pedro',	'123',	 1),
				('ESTEVAN NAVARRO LAGO',	'estevan',	'123',	 1),
				('HARRY	HPOTTER',			'potter',	'123',	 2),
				('POSTO 474',				'posto474',	'123',	 1),
				('POSTO VILLA SETTE',		'sette',	'123',	 2),
				('Administrador do Sistema','admin',	'admin', 1)
go

-- postos	-------------------------------------------------------------------

insert into postos (pessoa_id, cnpj, telefone)
	values	(1, '79.263.278/0001-01', '(17) 981492059'),
			(3, '42.931.155/0001-91', '(17) 981492059'),
			(7, '19.499.378/0001-00', '(17) 810362496'),
			(8, '92.773.681/0001-49', '(17) 925478123')

go

-- motoristas	---------------------------------------------------------------

insert into motoristas (pessoa_id, cpf, credito)
	values	(2, '924.813.930-23',     1200),
			(4, '471.874.450-40',     5000), 
			(5, '027.910.030-29',     0.50), 
			(6, '374.675.570-00', -2800.30)
go

insert into administradores (pessoa_id) 
	values	( 9 )
go


-- emails	-------------------------------------------------------------------

insert into emails values(1, 'pinattoposto@gmail.com')
insert into emails values(1, 'autopostopinatto@hotmail.com')
insert into emails values(2, 'eliasmantovan@fatec.sp.gov.br')
insert into emails values(3, 'cinquentao@yahoo.com.br')
insert into emails values(4, 'pedro@fatec.sp.gov.br')
insert into emails values(4, 'coimbra@gmail.com')
insert into emails values(4, 'sposito@bol.com.br')
insert into emails values(6, 'harry@fantasy.com.br')
insert into emails values(8, 'villasette@hotmail.com')


-- enderecos	---------------------------------------------------------------

insert into enderecos (posto_id, cep, localidade, cidade_id)  
		values	(1, '15055-200', 'Paulo Vidalli, 285', 5264),
				(3, '15050-453', 'XV de Novembro, 1515', 5270),
				(7, '15054-030', 'Rodovia Euclides da Cunha, km 477', 5302),
				(8, '15046-778', 'Santos Dumont, 3281', 4814)
go

-- combustiveis	---------------------------------------------------------------

insert into combustiveis (posto_id, nome, status)  
				values	(1,'ÁLCOOL',             1),
						(1,'GASOLINA',           1),
						(1,'GASOLINA ADITIVADA', 2),
						(1,'DIESEL S10',         1),
						(1,'DIESEL S500',        1),
						(3,'ÁLCOOL',             1),
						(3,'GASOLINA',           1),
						(3,'GASOLINA ADITIVADA', 2),
						(3,'DIESEL S10',         1),
						(3,'DIESEL S500',        1),
						(3,'ARLA',               1),
						(3,'ADITIVO',            1),
						(3,'QUEROSENE',          2),
						(7,'ÁLCOOL',             1),
						(7,'GASOLINA',           1),
						(7,'GASOLINA ADITIVADA', 1),
						(7,'DIESEL S10',         1),
						(7,'DIESEL S500',        1),
						(8,'ÁLCOOL',             1),
						(8,'GASOLINA',           1),
						(8,'GASOLINA ADITIVADA', 1),
						(8,'DIESEL S10',         1),
						(8,'DIESEL S500',        1)
go	

-- abastecimentos	-----------------------------------------------------------

insert into abastecimentos (data, cupom_fiscal, valor_total, status, veiculo_placa, motorista_id, posto_id )
		values	(getdate(),	  '26597419017464',	0.0, 2, 'DTU3934', 5, 3),		--1
				(getdate()-1, '03647950546329',	0.0, 1, 'FIA1147', 2, 1 ),		--2
				(getdate()-2, '97501348564102',	0.0, 2,	'CCC1111', 6, 7 ),		--3
				(getdate()-3, '69871230459670', 0.0, 1, 'BRA4C44', 4, 8 )		--4
go


--		N--N	===============================================================
-- abastecimentos__combustiveis		-------------------------------------------

insert into abastecimentos__combustiveis (abastecimento_id, posto_id, combustivel_id, valor, litros)
		values	(1, 3, 6, 6.20, 52),
				(1, 3, 8, 3.86, 100),
				(1, 3, 12, 2.55, 20)
go

insert into abastecimentos__combustiveis (abastecimento_id, posto_id, combustivel_id, valor, litros)
		values	(2, 1, 1, 6.80, 30	 ),
				(2, 1, 3, 5.50, 15.84),
				(2, 1, 5, 30.00, 1	 )
go

insert into abastecimentos__combustiveis (abastecimento_id, posto_id, combustivel_id, valor, litros)
		values	(3, 7, 17, 4.10, 750),
				(3, 7, 16, 2.55, 250)
go

insert into abastecimentos__combustiveis (abastecimento_id, posto_id, combustivel_id, valor, litros)
		values	(4, 8, 19, 4.10, 500),
				(4, 8, 21, 2.55, 100)
go


-- UPDATE abastecimentos		-----------------------------------------------

update abastecimentos
set valor_total =
(
	select sum(valor * litros) 
	from abastecimentos__combustiveis 
	where	abastecimento_id = 1
)
where id = 1
go

update abastecimentos
set valor_total =
(
	select sum(valor * litros) 
	from abastecimentos__combustiveis 
	where	abastecimento_id = 2
)
where id = 2
go

update abastecimentos
set valor_total =
(
	select sum(valor * litros) 
	from abastecimentos__combustiveis 
	where	abastecimento_id = 3
)
where id = 3
go

update abastecimentos
set valor_total =
(
	select sum(valor * litros) 
	from abastecimentos__combustiveis 
	where	abastecimento_id = 4
)
where id = 4
go

