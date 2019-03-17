using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAres {

    public struct PlayerStuts {
        public int Kills;
        public int Deaths;
        public int Assists;
        public int DamageDealed;
        public int DamageTaken;
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamageableObject {

        public static List<Player> _references = new List<Player>();

        #region Variables

        [Header("References")]
        [SerializeField] GameObject[] _weaponInit;
        [SerializeField] Transform _weaponAncor;
        [SerializeField] Transform _weaponRotationAncor;
        [SerializeField] Image _healthBar;
        [SerializeField] Transform _weaponWheel;
        [SerializeField] GameObject _controleObject;
        [SerializeField] LayerMask _dashColiders;

        [Header("Balancing")]
        [SerializeField] int _maxHealth = 100;
        [SerializeField] float _dashForce = 2;
        [SerializeField] float _invulnerableDuration = 1;

        public PlayerStuts _stuts;

        IControle _controle;
        List<IWeapon> _weapons = new List<IWeapon>();
        List<IItem> _items = new List<IItem>();

        Dictionary<Collider2D, Vector2> _collisionNormals = new Dictionary<Collider2D, Vector2>();

        float _respawntTime = float.MaxValue;
        int _currentHealth;
        int _currentWeapon = 0;
        bool _isShooting = false;

        public Rigidbody2D _rig { get; private set; }

        #endregion
        #region Unity

        void Start() {
            DontDestroyOnLoad(this.gameObject);
            //GameManager test = GameManager._singelton;
            _rig = GetComponent<Rigidbody2D>();
            Init();
            _references.Add(this);
        }
        private void OnDestroy() {
            _references.Remove(this);
        }
        
        void Update() {
            _weaponRotationAncor.rotation = Quaternion.LookRotation(transform.forward,new Vector2(-_controle._dir.y,_controle._dir.x));//vektor irgendwie drehen, damit es in der 2d plain bleibt

            _healthBar.fillAmount = (float)_currentHealth / _maxHealth;
        }

        #endregion

        public void Init() {
            if (_controle == null) {
                if(_controleObject == null) {
                    DestroyImmediate(gameObject);
                    return;
                }
                _controle = _controleObject.GetComponent<IControle>();
                if(_controle == null) {
                    //Destroy(gameObject);
                    DestroyImmediate(gameObject);
                    return;
                }
            }
            
            _controle.StartShooting += StartShooting;
            _controle.StopShooting += StopShooting;
            _controle.Dash += Dash;

            _controle.SelectWeapon += SelectWeapon;
            _controle.ChangeWeapon += ChangeWeapon;
            _controle.UseItem += UseItem;
            _controle.Disconect += Disconect;

            GameObject tmp;
            IWeapon tmpInterface;
            foreach (var it in _weaponInit) {
                tmpInterface = it.GetComponent<IWeapon>();
                if (tmpInterface != null) {
                    tmp = Instantiate(it, _weaponAncor);
                    tmpInterface = tmp.GetComponent<IWeapon>();
                    tmpInterface.Init(this);
                    tmpInterface.SetActive(false);
                    _weapons.Add(tmpInterface);
                }
            }

            Respawn(transform.position);//hier die richtige position eingeben
            //WeaponIcons in WheaponWheel einfügen;
        }

        public void Respawn(Vector2 pos) {
            transform.position = pos;

            _currentHealth = _maxHealth;
            if(_currentWeapon != 0) {
                _weapons[_currentWeapon].SetActive(false);
                _currentWeapon = 0;
                _weapons[_currentWeapon].SetActive(true);
            }
            _respawntTime = Time.timeSinceLevelLoad;
            //für eine zeit unverwundbar machen
        }

        public bool TakeDamage(int damage, out int realDamage) {
            if(Time.timeSinceLevelLoad-_respawntTime < _invulnerableDuration) {
                realDamage = 0;
                return false;
            }
            if(damage > _currentHealth) {
                realDamage = _currentHealth;
                _currentHealth = 0;
                _stuts.DamageTaken += realDamage;
                _stuts.Deaths++;
                return true;
            } else {
                realDamage = damage;
                _currentHealth -= realDamage;
                _stuts.DamageTaken += realDamage;
                return false;
            }
        }

        void SelectWeapon(int selectedWeapon) {
            if (selectedWeapon >= _weapons.Count || selectedWeapon < 0)
                selectedWeapon = 0;
            _weaponWheel.gameObject.SetActive(true);

            //_weaponWheel.GetChild(selectedWeapon) highlight selected item
        }

        void ChangeWeapon(int newWeapon) {
            if (newWeapon < _weapons.Count && newWeapon >= 0) {
                _weapons[_currentWeapon].StopShooting();
                _weapons[_currentWeapon].SetActive(false);
                _currentWeapon = newWeapon;
                _weapons[_currentWeapon].SetActive(true);
                if (_isShooting)
                    _weapons[_currentWeapon].StartShooting();
                _weaponWheel.gameObject.SetActive(false);
            }
        }

        void StartShooting() {
            _weapons[_currentWeapon].StartShooting();
            _isShooting = true;
        }

        void StopShooting() {
            _isShooting = false;
            _weapons[_currentWeapon].StopShooting();
        }

        void UseItem(int item) {
            if(item < _items.Count && item >= 0) {
                _items[item].Activate();
                _items.Remove(_items[item]);
            }
        }

        void Dash() {
            Vector2 tmp = new Vector2(0, 0);
            foreach(var it in _collisionNormals) {
                tmp += it.Value;
            }
            _rig.AddForce(tmp.normalized * _dashForce);
        }

        void Disconect() {
            Destroy(this.gameObject);
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            IItem tmpItem = collision.GetComponent<IItem>();
            if (tmpItem != null) {
                _items.Add(tmpItem);
                tmpItem.Collect();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (_dashColiders.value == (_dashColiders | 1<<collision.gameObject.layer)) {//wir nehmen eine 1(true) und schieben es um collision.gameObject.layer nach links, nehmen dann die _dashColiders LayerMask, setzen dieses bool auf true und fragen dann ob dass was da rauskommt dass selbe ist wie die _dashColiders LayerMask
                Vector2 tmpNormal = new Vector2(0, 0);
                foreach (var it in collision.contacts) {
                    tmpNormal += it.normal;
                }
                _collisionNormals[collision.collider] = tmpNormal.normalized;
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (_dashColiders.value == (_dashColiders | 1 << collision.gameObject.layer)) {
                _collisionNormals.Remove(collision.collider);
            }
        }

        #endregion
    }
}