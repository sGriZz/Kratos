﻿namespace Kratos.Data
{
    public class Mute : ModeratorAction
    { 
        public bool Active { get; set; }

        public ulong UnmuteAtUnixTimestamp { get; set; }
    }
}
