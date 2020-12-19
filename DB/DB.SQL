CREATE TYPE disease_b AS ENUM ('Грипп', 'ОРВИ', 'COVID-19', 'Ветрянка', 'Корь', 'Вакцинация', 'Гигиена', 'Профилактика', 'Эпидемия', 'Вирусные заболевания');

CREATE TABLE articles
(
    title character varying(30),
    image text DEFAULT '/img/placeholder.jpg',
    description text,
    create_date date DEFAULT CURRENT_DATE,
    article_id SERIAL PRIMARY KEY,
    user_id integer references users (user_id)
);

CREATE TABLE chats
(
    name character varying(50),
    short_description text,
    chat_id SERIAL PRIMARY KEY
);

CREATE TABLE messages
(
    chat_id integer,
    user_id integer,
    m_text text
);

CREATE TABLE users
(
    login character varying(50),
    password character varying(50),
    salt  character varying(32),
    img text,
    user_id SERIAL PRIMARY KEY
);

create table articles_to_users
(
	article_id integer references articles (article_id),
	user_id integer references users (user_id),
	primary key(user_id, article_id)
);

create table chats_to_users
(
	chat_id int references chats (chat_id),
	user_id int references users (user_id),
	primary key(user_id, chat_id)
);

create table tags_to_articles
(
	article_id int references articles (article_id),
	tag disease_b
);

ALTER TABLE messages ADD CONSTRAINT fk_message_id FOREIGN KEY (chat_id)
        REFERENCES chats (chat_id) MATCH SIMPLE;

ALTER TABLE messages ADD CONSTRAINT fk_message_id1 FOREIGN KEY (user_id)
        REFERENCES users (user_id) MATCH SIMPLE;