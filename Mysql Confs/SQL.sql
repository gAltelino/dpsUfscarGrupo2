CREATE DATABASE dps;
USE dps;

CREATE TABLE funcionarios(
	-- DADOS PESSOAIS
    cpf BIGINT NOT NULL PRIMARY KEY,
    nome VARCHAR(200) NOT NULL,
    nascimento DATE,
    estado_civil VARCHAR(50),
	-- LOCALIZAÇÃO
    cep INT,
    endereco VARCHAR(200),
    bairro VARCHAR(100),
    cidade VARCHAR(100),
    estado VARCHAR(2),
	-- CONTATO
	telefone_residencial VARCHAR(20),
	telefone_comercial VARCHAR(20),
	telefone_celular VARCHAR(20),
    -- ACESSO
    senha VARCHAR(200),
    acesso DATETIME,
    email VARCHAR(200),
    ativo BOOL
);

CREATE TABLE rondas(
	id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
	cpf_ronda BIGINT NOT NULL,
    datahora DATETIME NOT NULL,
    latitude FLOAT NOT NULL,
	longitude FLOAT NOT NULL
);
ALTER TABLE rondas ADD CONSTRAINT fk_rondasfuncionario FOREIGN KEY ( cpf_ronda ) REFERENCES funcionarios ( cpf ) ;

CREATE TABLE clientes(
	-- DADOS PESSOAIS
    cpf BIGINT NOT NULL PRIMARY KEY,
    nome VARCHAR(200) NOT NULL,
    nascimento DATE,
    estado_civil VARCHAR(50),
	-- LOCALIZAÇÃO
    cep INT,
    endereco VARCHAR(200),
    bairro VARCHAR(100),
    cidade VARCHAR(100),
    estado VARCHAR(2),
	-- CONTATO
	telefone_residencial VARCHAR(20),
	telefone_comercial VARCHAR(20),
	telefone_celular VARCHAR(20),
    -- ACESSO
    senha VARCHAR(200),
    acesso DATETIME,
    email VARCHAR(200),
    ativo BOOL,
    -- EMERGENCIA
    senha_liberacao int,
    senha_panico int
);

CREATE TABLE ocorrencias(
	id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
	cpf_funcionario BIGINT NULL,
	cpf_cliente BIGINT NOT NULL,
    datahora DATETIME NOT NULL,
    latitude FLOAT NOT NULL,
	longitude FLOAT NOT NULL,
    senha_utilizada INT,
    historico VARCHAR(10000)
);
ALTER TABLE ocorrencias ADD CONSTRAINT fk_ocorrenciasfuncionario FOREIGN KEY ( cpf_funcionario ) REFERENCES funcionarios ( cpf ) ;
ALTER TABLE ocorrencias ADD CONSTRAINT fk_ocorrenciascliente FOREIGN KEY ( cpf_cliente ) REFERENCES clientes ( cpf ) ;

CREATE TABLE auditoria(
	id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
	cpf_auditoria BIGINT NOT NULL,
    latitude FLOAT NOT NULL,
	longitude FLOAT NOT NULL
);
ALTER TABLE auditoria ADD CONSTRAINT fk_auditoriafuncionario FOREIGN KEY ( cpf_auditoria ) REFERENCES funcionarios ( cpf ) ;
ALTER TABLE auditoria ADD CONSTRAINT fk_auditoriacliente FOREIGN KEY ( cpf_auditoria ) REFERENCES clientes ( cpf ) ;

insert into Funcionarios (nome, cpf, senha, ativo) values ('ANTONIO', 36471042809,'81DC9BDB52D04DC20036DBD8313ED055',1)
