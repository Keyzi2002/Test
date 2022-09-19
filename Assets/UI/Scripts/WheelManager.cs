using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class WheelManager
{
    public List<WheelPiece> wheelPiecePersent;
    List<WheelPiece> wheelPiecePersentCalculate = new List<WheelPiece>();
    public void StartRandom(UnityAction action) {
        wheelPiecePersentCalculate.Clear();
        foreach (WheelPiece item in wheelPiecePersent)
        {
            WheelPiece wheelPiece = new WheelPiece();
            wheelPiece.persent = item.persent;
            wheelPiece.rarity = item.rarity;
            wheelPiecePersentCalculate.Add(wheelPiece);
        }
        action();
    }
    public WheelPiece GetWheelPiece() {
        float randomReward = Random.Range(0, 100);
        for (int i = 0; i < wheelPiecePersentCalculate.Count; i++)
        {
            if (randomReward < wheelPiecePersentCalculate[i].persent)
            {
                CalculateAgainPersent(wheelPiecePersentCalculate[i].rarity);
                return wheelPiecePersentCalculate[i];
            }
            else
                randomReward -= wheelPiecePersentCalculate[i].persent;
        }
        CalculateAgainPersent(wheelPiecePersentCalculate[wheelPiecePersentCalculate.Count - 1].rarity);
        return wheelPiecePersentCalculate[wheelPiecePersentCalculate.Count - 1];
    }
    void CalculateAgainPersent(Rarity rarity) {
        float x = 100 / 8;
        x = x / CalculateTotalPersent(rarity);
        foreach (WheelPiece wheelPiece in wheelPiecePersentCalculate)
        {
            if (wheelPiece.rarity != rarity)
                wheelPiece.persent = wheelPiece.persent + x * wheelPiece.persent;
            else wheelPiece.persent -= 100/8;
        }
    }
    float CalculateTotalPersent(Rarity rarity) {
        float sum = 0;
        foreach (WheelPiece wheelPiece in wheelPiecePersentCalculate)
        {
            if (wheelPiece.rarity != rarity)
                sum += wheelPiece.persent;
        }
        return sum;
    }
} 
