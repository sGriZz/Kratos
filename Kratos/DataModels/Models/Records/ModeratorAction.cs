﻿using System.ComponentModel.DataAnnotations;

namespace Kratos.Data
{
    public abstract class ModeratorAction
    {
        [Key]
        public int Key { get; set; }

        public ulong GuildId { get; set; }

        public ulong SubjectId { get; set; }

        public ulong ModeratorId { get; set; }

        public ulong UnixTimestamp { get; set; }

        public string Reason { get; set; }
    }
}
