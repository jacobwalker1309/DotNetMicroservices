using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
		public IntegrationBaseEvent()
		{
			Id = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}

		public IntegrationBaseEvent(Guid id, DateTime createDate)
		{
			Id = id;
			CreationDate = createDate;
		}

		public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}