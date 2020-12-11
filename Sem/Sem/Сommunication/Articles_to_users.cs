namespace Sem.Сommunication
{
    public class Articles_to_users : Communication
    {
        public Articles_to_users(int article_id, int user_id)
        {
            SetId("articles_to_users", article_id, user_id, "article_id", "user_id");
        }
    }
}
