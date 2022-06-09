using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IUserUpdatedTimelineService
    {
        List<UserUpdatedTimelineItem> GetUserUpdatedTimeline(String projectNumber);
    }
}
