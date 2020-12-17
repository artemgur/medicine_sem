namespace Sem.Сommunication
{
    public class Chats_to_users : Communication
    {
        public Chats_to_users(int chat_id, int user_id)
        {
            SetId("chats_to_users", chat_id, user_id, "chat_id", "user_id");
        }
    }
}
