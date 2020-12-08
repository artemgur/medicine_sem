CREATE TYPE disease_b AS ENUM ('ГРИПП', 'ОРВИ', 'КОРОНАВИРУС');

CREATE SEQUENCE tb_articles_id_seq
 INCREMENT BY 1
 NO MAXVALUE
 NO MINVALUE
 CACHE 1;

CREATE SEQUENCE tb_chats_id_seq
 INCREMENT BY 1
 NO MAXVALUE
 NO MINVALUE
 CACHE 1;

CREATE SEQUENCE tb_users_id_seq
 INCREMENT BY 1
 NO MAXVALUE
 NO MINVALUE
 CACHE 1;

CREATE SEQUENCE tb_tags_id_seq
 INCREMENT BY 1
 NO MAXVALUE
 NO MINVALUE
 CACHE 1;

CREATE TABLE articles
(
    title character varying(30),
    image text,
    description text,
    short_description text,
    create_date date,
    article_id integer DEFAULT nextval('tb_articles_id_seq') NOT NULL,
    PRIMARY KEY (article_id)
);

CREATE TABLE chats
(
    name character varying(50),
    short_description text,
    chat_id integer DEFAULT nextval('tb_chats_id_seq') NOT NULL,
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
    tag_id integer primary key DEFAULT nextval('tb_tags_id_seq') NOT NULL,
);

CREATE TABLE users
(
    login character varying(50),
    password character varying(50),
    salt  character varying(32),
    img text,
    user_id integer DEFAULT nextval('tb_users_id_seq') NOT NULL,
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