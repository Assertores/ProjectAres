﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Rocket : MonoBehaviour, IHarmingObject {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_explosionRef;

        [Header("Balancing")]
        [Tooltip("distance form levelorigion to autodestry")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] int m_damage = 1;

        Player m_source;
        Rigidbody2D m_rb;

        #endregion
        #region MonoBehaviour
        void Start() {

        }

        void Update() {
            if (Vector2.Distance(transform.position, Vector2.zero) > m_killDistance) {
                Destroy(gameObject);
            }
        }

        #endregion
        #region IHarmingObject

        public Rigidbody2D Init(Player reverence) {
            m_rb = GetComponent<Rigidbody2D>();
            if (!m_rb) {
                Destroy(gameObject);
                return null;
            }

            m_source = reverence;
            return m_rb;
        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            if(collision.gameObject == m_source.gameObject) {
                return;
            }

            if (m_explosionRef) {
                GameObject temp = Instantiate(m_explosionRef,transform.position, transform.rotation);
                temp.GetComponentInChildren<IHarmingObject>()?.Init(m_source);
            }

            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.TakeDamage(m_damage, m_source, Vector2.zero/*m_rb.velocity * m_rb.mass*/);//rocket wont give recoil to the hit one
            }
            Destroy(gameObject);
        }

        #endregion
    }
}