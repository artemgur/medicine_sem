using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Forum : PageModel
	{
        public List<object> ChatPars;
        public List<List<object>> messages;
		public int CountForumToUser;

		public void OnGet(int index)
        {
            ChatPars = DataBase.Select("SELECT * FROM chats WHERE chat_id = " + index + ";")[0];
            messages = DataBase.Select("SELECT * FROM messages WHERE chat_id = " + index + ";");
			if (Request.Cookies["user_id"] != null)
				CountForumToUser = DataBase.SelectCheck<int>("SELECT * FROM chats_to_users WHERE user_id = " + Request.Cookies["user_id"] + "AND chat_id = " + index + " LIMIT 1;").Count;
		}

        public void OnPostInsert(string forumId, string text)
        {
            OnGet(int.Parse(forumId));
            DataBase.Add("INSERT INTO messages VALUES (" + forumId + ", " + Request.Cookies["user_id"] + ", \'" + DataBase.ReplacingChars(text) + "\');");
		}

		public void OnPostAdd(int index)
		{
			OnGet(index);
			if (CountForumToUser == 0)
				DataBase.Add("INSERT INTO chats_to_users (user_id, chat_id) VALUES (" + Request.Cookies["user_id"] + ", " + index + "); ");
		}

		public void OnPostRemove(int index)
		{
			OnGet(index);
			if (CountForumToUser != 0)
				DataBase.Add("DELETE FROM chats_to_users WHERE user_id = " + Request.Cookies["user_id"] + " AND chat_id = " + index + ";");
		}
	}
}