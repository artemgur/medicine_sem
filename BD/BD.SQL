CREATE TYPE disease_b AS ENUM ('ГРИПП', 'ОРВИ', 'КОРОНАВИРУС');

CREATE TABLE articles
(
    title character varying(30),
    image text DEFAULT '/img/placeholder.jpg',
    description text,
    create_date date DEFAULT CURRENT_DATE,
    article_id SERIAL PRIMARY KEY,
    PRIMARY KEY (article_id)
);

CREATE TABLE chats
(
    name character varying(50),
    short_description text,
    chat_id SERIAL PRIMARY KEY,
    PRIMARY KEY (chat_id)
);

CREATE TABLE messages
(
    chat_id integer,
    user_id integer,
    m_text text
);

CREATE TABLE tags
(
    tag disease_b,
    color text
    tag_id SERIAL PRIMARY KEY,
);

CREATE TABLE users
(
    login character varying(50),
    password character varying(50),
    salt  character varying(32),
    img text,
    user_id SERIAL PRIMARY KEY,
    PRIMARY KEY (user_id)
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
	tag_id int references tags (tag_id),
	article_id int references articles (article_id),
	primary key(tag_id, article_id)
);

ALTER TABLE messages ADD CONSTRAINT fk_message_id FOREIGN KEY (chat_id)
        REFERENCES chats (chat_id) MATCH SIMPLE;

ALTER TABLE messages ADD CONSTRAINT fk_message_id FOREIGN KEY (user_id)
        REFERENCES users (user_id) MATCH SIMPLE;