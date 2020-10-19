﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Field : MonoBehaviour
{
    private readonly Data data = new Data();
    private Stack<Token> stack = new Stack<Token>();


    [Header("Container settings")]
    public bool hasContainer;


    [Header("Field info")]
    public Token[] tokenChilds;
    public Color colorTop;

    private void Update()
    {
        UpdateTokens();
        UpdatePosToGoZ();

        if (tokenChilds.Length > 0)
        {
            colorTop = tokenChilds[0].GetComponent<Image>().color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasContainer)
        {
            return;
        }
        if (collision.CompareTag("Token"))
        {
            Token tok = collision.GetComponent<Token>();
            if (tok.isNew)
            {
                tok.isNew = false;
                stack.Push(tok);
            }
        }
    }


    //hay que quitar los que sean missing...
    private void UpdateTokens()
    {

        //tokenChilds;
        Stack<Token> recipe_stack = new Stack<Token>();
        Token[] recipe_token = stack.ToArray();

        //esta haciendo un push alreves....
        //los push siempre van al inicio, en vez de al final...
        for (int x = recipe_token.Length; x > 0; x--)
        {
            if (!!recipe_token[x - 1])
            {
                recipe_stack.Push(recipe_token[x - 1]);
            }
            
        }

        stack = recipe_stack;
        tokenChilds = recipe_stack.ToArray();
        VerifyLastToken();
    }


    /// <summary>
    /// Partiendo de que [0] es la ultima ficha que poseemos podemos saber la ultima..., en caso de que
    /// sea agarrada o, no este en su posición, se puede permanecer como enable
    /// </summary>
    private void VerifyLastToken()
    {
        for (int x = 0; x < tokenChilds.Length; x++)
        {
            bool condition = x == 0
                || tokenChilds[x].isDraggin
                || !tokenChilds[x].posToGo.Equals(tokenChilds[x].transform.position);
                //|| !tokenChilds[x].isReached;
            tokenChilds[x].enabled = condition;

            //tokenChilds[x].img.enabled = condition || x == 1;


        }
    }



    private void UpdatePosToGoZ()
    {
        for (int x = 0; x < tokenChilds.Length; x++)
        {
            Vector2 pos2D = transform.position;
            Vector2 token_pos2D = tokenChilds[x].transform.position;

            
                
            if (token_pos2D.Equals(pos2D))
            {
                float X = tokenChilds[x].posToGo.x;
                float Y = tokenChilds[x].posToGo.y;
                float Z = data.tokenPosInit_z + (data.separeMagnitude * x);

                tokenChilds[x].posToGo = new Vector3(X, Y, Z);
            }
        }
    }
}