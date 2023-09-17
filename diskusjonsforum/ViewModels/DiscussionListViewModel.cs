using System;
using diskusjonsforum.Models;

namespace diskusjonsforum.ViewModels
{
	public class DiscussionListViewModel
	{
		public IEnumerable<DiscussionThread> DiscussionThreads;
		public string? CurrentViewName;

		public DiscussionListViewModel(IEnumerable<DiscussionThread> discussionThreads, string? currentViewName)
		{
			DiscussionThreads = discussionThreads;
			CurrentViewName = currentViewName;

		}
	}
}

