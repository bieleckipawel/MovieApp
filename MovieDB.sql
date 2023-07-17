CREATE DATABASE [MovieDB]
GO
USE [MovieDB]
GO
/****** Object:  Table [dbo].[filmy]    Script Date: 17.07.2023 22:39:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[filmy](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [nvarchar](50) NOT NULL,
	[opis] [text] NULL,
	[rok_premiery] [smallint] NULL,
	[budzet] [int] NULL,
	[id_gatunek] [int] NOT NULL,
	[id_rezyser] [int] NOT NULL,
 CONSTRAINT [PK_film] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[gatunki]    Script Date: 17.07.2023 22:39:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gatunki](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [nchar](10) NOT NULL,
	[opis] [text] NULL,
 CONSTRAINT [PK_gatunki] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[oceny]    Script Date: 17.07.2023 22:39:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[oceny](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_film] [int] NOT NULL,
	[id_uzytkownik] [int] NOT NULL,
	[ocena] [smallint] NOT NULL,
	[opis] [text] NULL,
 CONSTRAINT [PK_oceny] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rezyserowie]    Script Date: 17.07.2023 22:39:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rezyserowie](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[imie] [nvarchar](30) NOT NULL,
	[nazwisko] [nvarchar](50) NOT NULL,
	[kraj] [nvarchar](4) NULL,
	[data_urodzenia] [date] NULL,
 CONSTRAINT [PK_rezyserowie] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[uzytkownicy]    Script Date: 17.07.2023 22:39:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[uzytkownicy](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nickname] [nvarchar](15) NOT NULL,
	[imie] [nvarchar](30) NULL,
	[nazwisko] [nvarchar](50) NULL,
	[ulubiony_gatunek] [int] NULL,
	[password] [varchar](255) NULL,
 CONSTRAINT [PK_uzytkownicy] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[filmy] ON 
GO
INSERT [dbo].[filmy] ([id], [nazwa], [opis], [rok_premiery], [budzet], [id_gatunek], [id_rezyser]) VALUES (1, N'Zielona Mila', N'"Zielona mila" (tytul oryginalny: "The Green Mile") to film z 1999 roku w rezyserii Franka Darabonta, oparty na powiesci Stephena Kinga. Akcja toczy sie w wiezieniu w stanie Luizjana, gdzie wiezniowie oczekuja na wykonanie wyroku smierci. Historia skupia sie na Johnie Coffey''u, wiezniu o tajemniczych mocach uzdrawiania, oraz na strazniku wieziennym Paulu Edgecombie, który staje sie jego przyjacielem. Film porusza tematy milosci, ludzkiej natury, niesprawiedliwosci i wybaczenia, pozostawiajac widza z glebokimi refleksjami.', 1999, 60000000, 1, 1)
GO
INSERT [dbo].[filmy] ([id], [nazwa], [opis], [rok_premiery], [budzet], [id_gatunek], [id_rezyser]) VALUES (2, N'Forrest Gump', N'"Forrest Gump" to film z 1994 roku w rezyserii Roberta Zemeckisa, oparty na powiesci o tym samym tytule autorstwa Winstona Grooma. Film jest historia zycia Forresta Gumpa, mezczyzny o niskim ilorazie inteligencji, który przypadkowo bierze udzial w wielu waznych wydarzeniach historycznych w Stanach Zjednoczonych. Jego proste podejscie do zycia i szczere serce sprawiaja, ze zdobywa przyjaznie i wplywa na zycie wielu ludzi wokól niego.', 1994, 55000000, 2, 2)
GO
INSERT [dbo].[filmy] ([id], [nazwa], [opis], [rok_premiery], [budzet], [id_gatunek], [id_rezyser]) VALUES (3, N'Leon zawodowiec', N'"Leon zawodowiec" (tytul oryginalny: "Léon: The Professional") to francusko-amerykanski film z 1994 roku w rezyserii Luca Bessona. Film jest opowiescia o niecodziennym zwiazku miedzy mloda dziewczynka, Matilda (grana przez Natalie Portman), a zawodowym zabójca o imieniu Léon (grany przez Jeana Reno). Matilda staje sie podopieczna Léona po tym, jak jej rodzina zostaje zamordowana przez skorumpowanego policjanta, a on sam staje sie jej mentorem, uczac ja sztuki przetrwania i umiejetnosci zabójcy.', 1994, 16000000, 1, 3)
GO
INSERT [dbo].[filmy] ([id], [nazwa], [opis], [rok_premiery], [budzet], [id_gatunek], [id_rezyser]) VALUES (4, N'Requiem dla snu', N'
"Requiem dla snu" (tytul oryginalny: "Requiem for a Dream") to amerykanski film z 2000 roku w rezyserii Darrena Aronofsky''ego. Film jest oparty na powiesci Huberta Selby''ego Jr. o tym samym tytule. "Requiem dla snu" to brutalna i poruszajaca historia czterech bohaterów – Harry''ego, jego matki Sare, jego dziewczyny Marion oraz jego przyjaciela Tyrone''a, którzy borykaja sie z uzaleznieniem od narkotyków. Film przedstawia rozpad ludzkich marzen i ambicji, a takze negatywne konsekwencje ich wyborów.', 2000, 4500000, 1, 4)
GO
INSERT [dbo].[filmy] ([id], [nazwa], [opis], [rok_premiery], [budzet], [id_gatunek], [id_rezyser]) VALUES (5, N'Matrix', N'"Matrix" to amerykanski film science fiction z 1999 roku w rezyserii Wachowskich (Lilly i Lana Wachowski). Film opowiada historie Thomasa "Nea" Andersona, programisty komputerowego, który odkrywa, ze swiat, w którym zyje, jest jedynie symulacja stworzona przez inteligentne maszyny, aby kontrolowac ludzkosc. Dowiaduje sie, ze jest wybrany do walki przeciwko maszynom i do wyzwolenia ludzkosci, wchodzac w tajemniczy wirtualny swiat znanego jako "Matrix".', 1999, 63000000, 4, 5)
GO
INSERT [dbo].[filmy] ([id], [nazwa], [opis], [rok_premiery], [budzet], [id_gatunek], [id_rezyser]) VALUES (1003, N'Skazani na Shawshank', N'"Skazani na Shawshank" (oryginalny tytul: "The Shawshank Redemption") to film z 1994 roku, wyrezyserowany przez Franka Darabonta, oparty na opowiadaniu Stephena Kinga o tym samym tytule. Film zostal uznany za jedno z najwybitniejszych dziel w historii kina i zdobyl liczne nagrody oraz uznanie krytyków.', 1994, 25000000, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[filmy] OFF
GO
SET IDENTITY_INSERT [dbo].[gatunki] ON 
GO
INSERT [dbo].[gatunki] ([id], [nazwa], [opis]) VALUES (1, N'Dramat    ', N' zapozyczone z literatury okreslenie filmów, których nie da sie zakwalifikowac do okreslonych gatunków filmowych. Kwalifikowanie pewnych filmów jako „dramaty” wynika z braku konwencji charakteryzujacych kino gatunkowe, a takze z wlasciwosci artystycznych danego dziela')
GO
INSERT [dbo].[gatunki] ([id], [nazwa], [opis]) VALUES (2, N'Komedia   ', N'film przedstawiajacy sytuacje i postacie wywolujace u widzów efekt komiczny. Komedia istnieje od zarania dziejów sztuki filmowej – za pierwszy film komediowy uchodzi Polewacz polany autorstwa braci Lumiere.')
GO
INSERT [dbo].[gatunki] ([id], [nazwa], [opis]) VALUES (3, N'Horror    ', N'odmiana fantastyki polegajaca na budowaniu swiata przedstawionego na wzór rzeczywistosci i praw nia rzadzacych po to, aby wprowadzic w jego obreb zjawiska kwestionujace te prawa i niedajace sie wytlumaczyc bez odwolywania sie do zjawisk nadprzyrodzonych.')
GO
INSERT [dbo].[gatunki] ([id], [nazwa], [opis]) VALUES (4, N'Akcja     ', N'')
GO
INSERT [dbo].[gatunki] ([id], [nazwa], [opis]) VALUES (5, N'Wojenny   ', N'wojenny')
GO
SET IDENTITY_INSERT [dbo].[gatunki] OFF
GO
SET IDENTITY_INSERT [dbo].[oceny] ON 
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2003, 1, 1, 5, N'OK')
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2004, 2, 1, 4, N'OK')
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2005, 3, 1, 3, N'OK')
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2006, 4, 1, 2, N'OK')
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2007, 5, 1, 1, N'OK')
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2008, 1003, 1, 1, N'OK')
GO
INSERT [dbo].[oceny] ([id], [id_film], [id_uzytkownik], [ocena], [opis]) VALUES (2009, 1003, 2003, 4, N'OK')
GO
SET IDENTITY_INSERT [dbo].[oceny] OFF
GO
SET IDENTITY_INSERT [dbo].[rezyserowie] ON 
GO
INSERT [dbo].[rezyserowie] ([id], [imie], [nazwisko], [kraj], [data_urodzenia]) VALUES (1, N'Frank', N'Darabont', N'FR', CAST(N'1959-01-28' AS Date))
GO
INSERT [dbo].[rezyserowie] ([id], [imie], [nazwisko], [kraj], [data_urodzenia]) VALUES (2, N'Robert', N'Zemeckis', N'US', CAST(N'1951-05-14' AS Date))
GO
INSERT [dbo].[rezyserowie] ([id], [imie], [nazwisko], [kraj], [data_urodzenia]) VALUES (3, N'Luc', N'Besson', N'FR', CAST(N'1959-03-18' AS Date))
GO
INSERT [dbo].[rezyserowie] ([id], [imie], [nazwisko], [kraj], [data_urodzenia]) VALUES (4, N'Darren', N'Aronofsky', N'US', CAST(N'1969-02-12' AS Date))
GO
INSERT [dbo].[rezyserowie] ([id], [imie], [nazwisko], [kraj], [data_urodzenia]) VALUES (5, N'Lilly', N'Wachowski', N'US', CAST(N'1967-12-29' AS Date))
GO
INSERT [dbo].[rezyserowie] ([id], [imie], [nazwisko], [kraj], [data_urodzenia]) VALUES (1002, N'Patryk', N'Vega', N'PL', CAST(N'1977-01-02' AS Date))
GO
SET IDENTITY_INSERT [dbo].[rezyserowie] OFF
GO
SET IDENTITY_INSERT [dbo].[uzytkownicy] ON 
GO
INSERT [dbo].[uzytkownicy] ([id], [nickname], [imie], [nazwisko], [ulubiony_gatunek], [password]) VALUES (1, N'user', N'Jan', N'Nowak', 2, N'pass')
GO
INSERT [dbo].[uzytkownicy] ([id], [nickname], [imie], [nazwisko], [ulubiony_gatunek], [password]) VALUES (2003, N'user2', N'Anna', N'Nowak', 1, N'pass')
GO
SET IDENTITY_INSERT [dbo].[uzytkownicy] OFF
GO
ALTER TABLE [dbo].[filmy]  WITH CHECK ADD FOREIGN KEY([id_gatunek])
REFERENCES [dbo].[gatunki] ([id])
GO
ALTER TABLE [dbo].[filmy]  WITH CHECK ADD FOREIGN KEY([id_rezyser])
REFERENCES [dbo].[rezyserowie] ([id])
GO
ALTER TABLE [dbo].[oceny]  WITH CHECK ADD FOREIGN KEY([id_film])
REFERENCES [dbo].[filmy] ([id])
GO
ALTER TABLE [dbo].[oceny]  WITH CHECK ADD FOREIGN KEY([id_uzytkownik])
REFERENCES [dbo].[uzytkownicy] ([id])
GO
ALTER TABLE [dbo].[uzytkownicy]  WITH CHECK ADD FOREIGN KEY([ulubiony_gatunek])
REFERENCES [dbo].[gatunki] ([id])
GO
