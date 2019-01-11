﻿using System;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Text;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Notifications;

namespace Eventos.IO.Domain.CommandHandlers
{
    public abstract class CommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;
        protected readonly IBus _bus;

        public CommandHandler(IUnitOfWork uow, IBus bus, IDomainNotificationHandler<DomainNotification> notifications)
        {
            _uow = uow;
            _bus = bus;
            _notifications = notifications;
        }

        protected void NotificarValidacoesErro(ValidationResult validationResult)
        {
            foreach(var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
                _bus.RaiseEvent(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }

        protected bool Commit()
        {
            // TODO: Verificar se ha alguma validacao de negocio com erro

            if (_notifications.HasNotifications())
                return false;

            var commandResponse = _uow.Commit();

            if (commandResponse.Success)
                return true;

            Console.WriteLine("Ocorreu um erro ao salvar os dados no banco");

            _bus.RaiseEvent(new DomainNotification("Commit", "Ocorreu um erro ao salvar os dados no banco"));

            return false;
        }
    }
}
