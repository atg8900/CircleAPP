using System;
using System.Collections.Generic;
using System.Text;

namespace CircleApp.Data.Services
{
    public interface IHashtagService
    {
        Task ProcessHashtagsForNewPostAsync(string content);

        Task ProcessHashtagsForRemovedPostAsync(string content);
    }
}
