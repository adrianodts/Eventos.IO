﻿namespace Eventos.IO.Domain.Core.Commands
{
    public class CommandResponse
    {
        public static CommandResponse OK = new CommandResponse { Success = true };
        public static CommandResponse Fail = new CommandResponse { Success = false };
        public bool Success { get; private set; }

        public CommandResponse(bool success = false)
        {
            Success = success;
        }
    }
}