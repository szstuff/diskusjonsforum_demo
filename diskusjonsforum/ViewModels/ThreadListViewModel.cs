using System;
using diskusjonsforum.Models;
using Thread = diskusjonsforum.Models.Thread;

namespace diskusjonsforum.ViewModels
{
	public class ThreadListViewModel
	{
		public IEnumerable<Thread> DiscussionThreads;
		public string? CurrentViewName;

		public ThreadListViewModel(IEnumerable<Thread> discussionThreads, string? currentViewName)
		{
			DiscussionThreads = discussionThreads;
			CurrentViewName = currentViewName;

		}
	}
}

