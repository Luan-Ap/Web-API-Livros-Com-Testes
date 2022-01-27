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
	('O Rei do Inverno', 'O rei do inverno conta a mais fiel história do lendário guerreiro Artur, que entrou para a história com o título de rei, embora nunca tenha usado uma coroa. A partir de fatos, este romance genial retrata o maior de todos os heróis como um poderoso guerreiro britânico, que luta contra os saxões para manter unida a Britânia, no século V, após a saída dos romanos.', '1995/10/05', 'Bernard Cornwell'),
	('Harry Potter e a Pedra Filosofal', 'Harry Potter e a Pedra Filosofal é o primeiro dos sete livros da série de fantasia Harry Potter, escrita por J. K. Rowling. O livro conta a história de Harry Potter, um órfão criado pelos tios que descobre, em seu décimo primeiro aniversário, que é um bruxo.', '1997/06/26', 'J. K. Rowling'),
	('Harry Potter e a Câmara Secreta', 'É o segundo livro da série Harry Potter. O livro se envolve em torno da lenda de uma câmara secreta localizada na Escola de Magia e Bruxaria de Hogwarts, na qual abriga um monstro que matará a todos os bruxos que não provém de famílias mágicas. Diversos alunos aparecem petrificados e Harry Potter, além de ser apontado como o maior suspeito, tenta desvendar e resolver o mistério junto de seus melhores amigos, Rony Weasley e Hermione Granger.', '1998-07-02', 'J. K. Rowling')
GO