using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class testClick : MonoBehaviour
    {
        private int c, b = 5;
        private int a, d = 8;
        private int r = 0;
        Queue<int> nums = new Queue<int>();

        private void Start() {
            print($"c = {c}");
            print($"b = {b}");
            nums.Enqueue(r);
            nums.Enqueue(a);
            nums.Enqueue(b);
            nums.Dequeue();
            nums.Enqueue(c);
            nums.Dequeue();
            nums.Enqueue(d);
            nums.Dequeue();

            foreach (int num in nums) {
                print(num);
            }
        }

        public void OnClick()
        {
            Debug.Log("Click");

        }
    }
}
