USE master;
GO

CREATE DATABASE DB_LIVROS;
GO

USE DB_LIVROS;
GO

CREATE TABLE Livros(
	LivroId int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Titulo varchar(100) NOT NULL,
	Sinopse varchar(450) NOT NULL,
	Publicacao date NOT NULL,
	Autor varchar(100) NOT NULL,
);
GO

INSERT INTO Livros (Titulo, Sinopse, Publicacao, Autor) VALUES
	('O Rei do Inverno', 'O rei do inverno conta a mais fiel hist�ria do lend�rio guerreiro Artur, que entrou para a hist�ria com o t�tulo de rei, embora nunca tenha usado uma coroa. A partir de fatos, este romance genial retrata o maior de todos os her�is como um poderoso guerreiro brit�nico, que luta contra os sax�es para manter unida a Brit�nia, no s�culo V, ap�s a sa�da dos romanos.', '1995/10/05', 'Bernard Cornwell'),
	('Harry Potter e a Pedra Filosofal', 'Harry Potter e a Pedra Filosofal � o primeiro dos sete livros da s�rie de fantasia Harry Potter, escrita por J. K. Rowling. O livro conta a hist�ria de Harry Potter, um �rf�o criado pelos tios que descobre, em seu d�cimo primeiro anivers�rio, que � um bruxo.', '1997/06/26', 'J. K. Rowling'),
	('Harry Potter e a C�mara Secreta', '� o segundo livro da s�rie Harry Potter. O livro se envolve em torno da lenda de uma c�mara secreta localizada na Escola de Magia e Bruxaria de Hogwarts, na qual abriga um monstro que matar� a todos os bruxos que n�o prov�m de fam�lias m�gicas. Diversos alunos aparecem petrificados e Harry Potter, al�m de ser apontado como o maior suspeito, tenta desvendar e resolver o mist�rio junto de seus melhores amigos, Rony Weasley e Hermione Granger.', '1998-07-02', 'J. K. Rowling')
GO