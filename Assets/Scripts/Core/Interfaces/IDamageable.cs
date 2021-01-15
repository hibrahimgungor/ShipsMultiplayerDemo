using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamageable
{
    /// <summary>
    /// Objenin anlık olarak sahip olduğu can puanı
    /// </summary>
    int health { get; }

    /// <summary>
    /// Obje belirtilen miktarda hasar alır.
    /// </summary>
    /// <param name="amount">Alınacak hasar miktarı</param>
    void TakeDamage(int amount);

}
