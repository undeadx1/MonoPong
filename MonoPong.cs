
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GeirDev
{
    public class Spark
    {
        public Vector2 position;
        public float timer;
        public float duration;

        public Spark(Vector2 position, float duration)
        {
            this.position = position;
            this.timer = 0;
            this.duration = duration;
        }

        public bool Update(float deltaTime)
        {
            timer += deltaTime;
            return timer < duration;
        }
    }
    
    public class MonoPong : MonoBehaviour
    {
        private List<Vector2> ballTrailPositions = new List<Vector2>();
        private int maxTrailLength = 10;

        public RawImage outputImage;

        private Texture2D gameTexture;
        private int textureWidth;
        private int textureHeight;

        private Vector2 ballPosition;
        private Vector2 ballVelocity;

        private float paddleHeight = 30;
        private float paddleWidth = 5;

        private Rect leftPaddle;
        private Rect rightPaddle;

        private List<Spark> sparks = new List<Spark>();
        private float sparkDuration = 0.5f;

        void Start()
        {
            RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
            textureWidth = Mathf.RoundToInt(rectTransform.rect.width);
            textureHeight = Mathf.RoundToInt(rectTransform.rect.height);

            gameTexture = new Texture2D(textureWidth, textureHeight);
            outputImage.texture = gameTexture;

            InitializeGame();
        }

        void InitializeGame()
        {
            ballPosition = new Vector2(textureWidth / 2, textureHeight / 2);
            ballVelocity = new Vector2(4, 4);

            leftPaddle = new Rect(10,
                Mathf.Clamp(textureHeight / 2 - paddleHeight / 2, 0, textureHeight - paddleHeight), paddleWidth,
                paddleHeight);
            rightPaddle = new Rect(textureWidth - 20,
                Mathf.Clamp(textureHeight / 2 - paddleHeight / 2, 0, textureHeight - paddleHeight), paddleWidth,
                paddleHeight);
        }

        void Update()
        {
            UpdateBall();
            UpdatePaddles();
            DrawGame();
        }



        void UpdateBall()
        {
            ballTrailPositions.Insert(0, ballPosition);

            if (ballTrailPositions.Count > maxTrailLength)
            {
                ballTrailPositions.RemoveAt(maxTrailLength);
            }

            ballPosition += ballVelocity;

            if (ballPosition.y - 5 <= 0 || ballPosition.y + 5 >= textureHeight)
            {
                ballVelocity.y = -ballVelocity.y;
            }

            if (leftPaddle.Contains(ballPosition) || rightPaddle.Contains(ballPosition))
            {
                ballVelocity.x = -ballVelocity.x;
                sparks.Add(new Spark(ballPosition, sparkDuration));
            }

            if (ballPosition.x - 5 <= 0 || ballPosition.x + 5 >= textureWidth)
            {
                InitializeGame();
            }

            ballPosition.x = Mathf.Clamp(ballPosition.x, 5, textureWidth - 6);
            ballPosition.y = Mathf.Clamp(ballPosition.y, 5, textureHeight - 6);
        }



        void UpdatePaddles()
        {
            float reactionDistance = textureWidth / 3;

            if (ballPosition.x < reactionDistance)
            {
                if (ballPosition.y > leftPaddle.y + paddleHeight / 2)
                {
                    leftPaddle.y += 4;
                }
                else if (ballPosition.y < leftPaddle.y + paddleHeight / 2)
                {
                    leftPaddle.y -= 4;
                }
            }

            if (ballPosition.x > textureWidth - reactionDistance)
            {
                if (ballPosition.y > rightPaddle.y + paddleHeight / 2)
                {
                    rightPaddle.y += 4;
                }
                else if (ballPosition.y < rightPaddle.y + paddleHeight / 2)
                {
                    rightPaddle.y -= 4;
                }
            }

            leftPaddle.y = Mathf.Clamp(leftPaddle.y, 0, textureHeight - paddleHeight);
            rightPaddle.y = Mathf.Clamp(rightPaddle.y, 0, textureHeight - paddleHeight);
        }

        void DrawGame()
        {
            ClearTexture();

            DrawRect(leftPaddle, Color.white);
            DrawRect(rightPaddle, Color.white);

            for (int i = 0; i < ballTrailPositions.Count - 1; i++)
            {
                float alpha = (ballTrailPositions.Count - 1 - i) / (float)ballTrailPositions.Count;
                DrawCircle(ballTrailPositions[i], 5, new Color(1, 1, 1, alpha));
            }

            DrawCircle(ballPosition, 5, Color.white);

            for (int i = sparks.Count - 1; i >= 0; i--)
            {
                Spark spark = sparks[i];
                if (spark.Update(Time.deltaTime))
                {
                    DrawSpark(spark.position, 20, Color.yellow, spark.timer / spark.duration);
                }
                else
                {
                    sparks.RemoveAt(i);
                }
            }

            gameTexture.Apply();
        }

        void ClearTexture()
        {
            Color32[] clearColor = new Color32[textureWidth * textureHeight];
            for (int i = 0; i < clearColor.Length; i++)
            {
                clearColor[i] = Color.clear;
            }

            gameTexture.SetPixels32(clearColor);
        }

        void DrawRect(Rect rect, Color color)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; x++)
            {
                for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
                {
                    gameTexture.SetPixel(x, y, color);
                }
            }
        }

        void DrawSpark(Vector2 position, int sparkCount, Color color, float progress)
        {
            float fade = 1 - progress;
            for (int i = 0; i < sparkCount; i++)
            {
                float angle = Random.Range(0, 2 * Mathf.PI);
                float distance = progress * 8;
                int x = (int)(position.x + Mathf.Cos(angle) * distance);
                int y = (int)(position.y + Mathf.Sin(angle) * distance);

                Color fadedColor = new Color(color.r, color.g, color.b, color.a * fade);
                gameTexture.SetPixel(x, y, fadedColor);
            }
        }

        void DrawCircle(Vector2 center, float radius, Color color)
        {
            int xMin = Mathf.Clamp((int)(center.x - radius), 0, textureWidth - 1);
            int xMax = Mathf.Clamp((int)(center.x + radius), 0, textureWidth - 1);
            int yMin = Mathf.Clamp((int)(center.y - radius), 0, textureHeight - 1);
            int yMax = Mathf.Clamp((int)(center.y + radius), 0, textureHeight - 1);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    if (distance <= radius)
                    {
                        gameTexture.SetPixel(x, y, color);
                    }
                }
            }
        }
    }
}
