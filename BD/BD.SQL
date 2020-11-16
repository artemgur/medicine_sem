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

CREATE TABLE articles
(
    title character varying(30),
    image text,
    description text,
    short_description text,
    create_date date,
    user_id integer,
    article_id integer DEFAULT nextval('tb_articles_id_seq') NOT NULL,
    CONSTRAINT articles_pkey PRIMARY KEY (article_id)
);

CREATE TABLE chats
(
    name character varying(50),
    short_description text,
    user_id integer,
    chat_id integer DEFAULT nextval('tb_chats_id_seq') NOT NULL,
    CONSTRAINT chats_pkey PRIMARY KEY (chat_id)
);

CREATE TABLE messages
(
    chat_id integer,
    m_text text
);

CREATE TABLE tags
(
    tag disease_b,
    article_id integer
);

CREATE TABLE users
(
    login character varying(50),
    password character varying(50),
    user_id integer DEFAULT nextval('tb_users_id_seq') NOT NULL,
    CONSTRAINT users_pkey PRIMARY KEY (user_id)
);

ALTER TABLE articles ADD CONSTRAINT fk_arcticle_id FOREIGN KEY (user_id)
        REFERENCES users (user_id);

ALTER TABLE chats ADD CONSTRAINT fk_chat_id FOREIGN KEY (user_id)
        REFERENCES users (user_id) MATCH SIMPLE;

ALTER TABLE messages ADD CONSTRAINT fk_message_id FOREIGN KEY (chat_id)
        REFERENCES chats (chat_id) MATCH SIMPLE;

ALTER TABLE tags ADD CONSTRAINT fk_tag_id FOREIGN KEY (article_id)
        REFERENCES articles (article_id) MATCH SIMPLE;