using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

/* component to be added to the root/top-level game object for all enemies */
public class EnemyRoot : MonoBehaviour
{
    public EnemyComponents Enemy { get; private set; } = null;

    protected void Awake()
    {
        Enemy = GetComponent<EnemyComponents>() ?? GetComponentInChildren<EnemyComponents>();
        Assert.IsNotNull(Enemy, $"EnemyRoot object '{gameObject}' could not find it's EnemyAdditions object!");
    }
}