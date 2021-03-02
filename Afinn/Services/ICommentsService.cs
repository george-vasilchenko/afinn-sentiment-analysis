using System.Collections.Generic;
using Afinn.Models;

namespace Afinn.Services
{
    public interface ICommentsService
    {
        IEnumerable<Comment> GetComments();
    }
}