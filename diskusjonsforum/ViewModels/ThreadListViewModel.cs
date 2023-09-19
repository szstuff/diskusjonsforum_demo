using System;
using diskusjonsforum.Models;
using Thread = diskusjonsforum.Models.Thread;

namespace diskusjonsforum.ViewModels
{
	public class ThreadListViewModel
	{
		public IEnumerable<Thread> Threads;
		public string? CurrentViewName;

		public ThreadListViewModel(IEnumerable<Thread> threads, string? currentViewName)
		{
			Threads = threads;
			CurrentViewName = currentViewName;

		}
	}
}

