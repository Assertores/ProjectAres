using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface ITracer {

        /// <summary>
        /// reference to the source object
        /// </summary>
        IHarmingObject m_trace { get; }


        /// <summary>
        /// will be called once after instanciating
        /// </summary>
        /// <param name="trace">the source object</param>
        /// <returns>a rigitbody to aply a force or null</returns>
        Rigidbody2D Init(IHarmingObject trace);
    }
}