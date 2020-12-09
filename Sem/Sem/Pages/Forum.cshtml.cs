using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sem.ModelsTables;
using Sem.Сommunication;
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
        public Chat ChatPars;
        public List<Message> messages;
		public int CountForumToUser;

		public void OnGet(int index)
        {
            ChatPars = DB_Operations.ChatsOps.GetChat(index);
			messages = DB_Operations.MessageOps.GetChatMessages(index);
			if (Request.Cookies["user_id"] != null)
				CountForumToUser = new Chats_to_users(index, int.Parse(Request.Cookies["user_id"])).CountComs();
		}

        public void OnPostInsert(string forumId, string text)
        {
			var id = int.Parse(forumId);
			OnGet(id);
			DB_Operations.MessageOps.AddMessage(id, Request, text);
		}

		public void OnPostAdd(int index)
		{
			OnGet(index);
			if (CountForumToUser == 0)
				new Chats_to_users(index, int.Parse(Request.Cookies["user_id"])).Add();
		}

		public void OnPostRemove(int index)
		{
			OnGet(index);
			if (CountForumToUser != 0)
				new Chats_to_users(index, int.Parse(Request.Cookies["user_id"])).Remove();
		}
	}
}