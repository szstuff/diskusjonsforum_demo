using System;
using diskusjonsforum.Models;

namespace diskusjonsforum.ViewModels
{
	public class DiscussionListViewModel
	{
		public IEnumerable<Discussion> DiscussionThreads;
		public string? CurrentViewName;

		public DiscussionListViewModel(IEnumerable<Discussion> discussionThreads, string? currentViewName)
		{
			DiscussionThreads = discussionThreads;
			CurrentViewName = currentViewName;

		}
	}
}

