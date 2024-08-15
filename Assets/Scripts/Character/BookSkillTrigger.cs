using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookSkillTrigger : MonoBehaviour
{
    [SerializeField] private CircleQuery query;
    public Action OnSkillActivate;
    public List<Enemy> entities = new List<Enemy>();
    private PlayerEnergyHandler _playerEnergyHandler;
    public int dmg;
    public int damageDone;
    private bool _skillActivate;
    public int force;
    public ParticleSystem ps;
    public int hitCount = 3;

    private void Awake()
    {
        _playerEnergyHandler = GetComponent<PlayerEnergyHandler>();
    }

    private void Update()
    {
        if (Player.Instance.cantUse) return;
        
        if (Input.GetKeyDown(KeyCode.Q) && _playerEnergyHandler.HaveEnoughEnergy())//IA2-P2
        {
            Player.Instance.playerAnim.Shooting = false;
            Player.Instance.playerAnim.skillBook = true;
        }
    }


    public void StartExplosion()
    {
        entities = query.Query().Select(x => (Enemy)x).Where(x => x != null && !x.canBanish).ToList();
        Explosion(entities);
    }
    private void Explosion(List<Enemy> enemies)
    {
        if (!_playerEnergyHandler.HaveEnoughEnergy() || _skillActivate) return;
        _skillActivate = true;
        _playerEnergyHandler.ModifiedEnergy(-_playerEnergyHandler.maxAmount);
        damageDone = DamageDone(enemies);
        foreach (var entity in enemies)
        {
            entity.Life.TakeDamage(dmg, force, hitCount);
        }
        _skillActivate = false;
        OnSkillActivate?.Invoke();
    }
    
    int DamageDone(IEnumerable<Enemy> entities)//IA2-P1
    {
        return entities.Aggregate(0, (acum, current) =>
        {
            var actualLife = current.GetComponent<LifeHandler>().ActualLife;
            acum += actualLife > dmg ? dmg : dmg - actualLife;
            return acum;
        });
    }


}
