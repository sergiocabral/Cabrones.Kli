﻿using System;
using Kli.i18n;
using Kli.Input;
using Kli.Output;

namespace Kli.Module
{
    /// <summary>
    /// Agrupa funções utilitárias para lidar com a interação com o usuário.
    /// </summary>
    public class Interaction: IInteraction
    {
        /// <summary>
        /// Gerencia múltiplas instâncias da mesma interface: IOutput
        /// </summary>
        private readonly IMultipleOutput _multipleOutput;

        /// <summary>
        /// Gerencia múltiplas instâncias da mesma interface: IInput
        /// </summary>
        private readonly IMultipleInput _multipleInput;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="multipleOutput">Gerencia múltiplas instâncias da mesma interface: IOutput</param>
        /// <param name="multipleInput">Gerencia múltiplas instâncias da mesma interface: IInput</param>
        public Interaction(IMultipleOutput multipleOutput, IMultipleInput multipleInput)
        {
            _multipleOutput = multipleOutput;
            _multipleInput = multipleInput;
        }

        /// <summary>
        /// Indica que a interação com o usuário já foi iniciada.
        /// </summary>
        private bool _startedInteraction;
        
        /// <summary>
        /// Inicia a interação com o usuário.
        /// Só pode ser executado uma vez este método.
        /// </summary>
        public void StartInteraction()
        {
            if (_startedInteraction) throw new InvalidOperationException();
            _startedInteraction = true;
            
            _multipleOutput.WriteLine("Started.");
            _multipleOutput.WriteLine("Yes".Translate("pt"));
            _multipleInput.Read(true);
        }
    }
}