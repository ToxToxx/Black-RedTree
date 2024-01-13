namespace Black_RedTree
{
    using System;

    enum Color
    {
        Red,
        Black
    }

    class Node
    {
        public int Data;
        public Color Color;
        public Node Parent, Left, Right;

        public Node(int data, Color color)
        {
            Data = data;
            Color = color;
            Parent = Left = Right = null;
        }
    }

    class RedBlackTree
    {
        private Node root;

        // Вспомогательный метод для вращения узлов
        private void RotateLeft(Node x)
        {
            Node y = x.Right;
            x.Right = y.Left;

            if (y.Left != null)
                y.Left.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == null)
                root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }

        // Вспомогательный метод для вращения узлов
        private void RotateRight(Node x)
        {
            Node y = x.Left;
            x.Left = y.Right;

            if (y.Right != null)
                y.Right.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == null)
                root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Right = x;
            x.Parent = y;
        }

        // Вспомогательный метод для поддержания баланса после вставки
        private void InsertFixup(Node z)
        {
            while (z.Parent != null && z.Parent.Color == Color.Red)
            {
                if (z.Parent == z.Parent.Parent.Left)
                {
                    Node y = z.Parent.Parent.Right;

                    if (y != null && y.Color == Color.Red)
                    {
                        z.Parent.Color = Color.Black;
                        y.Color = Color.Black;
                        z.Parent.Parent.Color = Color.Red;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Right)
                        {
                            z = z.Parent;
                            RotateLeft(z);
                        }

                        z.Parent.Color = Color.Black;
                        z.Parent.Parent.Color = Color.Red;
                        RotateRight(z.Parent.Parent);
                    }
                }
                else
                {
                    Node y = z.Parent.Parent.Left;

                    if (y != null && y.Color == Color.Red)
                    {
                        z.Parent.Color = Color.Black;
                        y.Color = Color.Black;
                        z.Parent.Parent.Color = Color.Red;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Left)
                        {
                            z = z.Parent;
                            RotateRight(z);
                        }

                        z.Parent.Color = Color.Black;
                        z.Parent.Parent.Color = Color.Red;
                        RotateLeft(z.Parent.Parent);
                    }
                }
            }

            root.Color = Color.Black;
        }

        // Основной метод для вставки узла в дерево
        public void Insert(int data)
        {
            Node z = new Node(data, Color.Red);
            Node y = null;
            Node x = root;

            while (x != null)
            {
                y = x;

                if (z.Data < x.Data)
                    x = x.Left;
                else
                    x = x.Right;
            }

            z.Parent = y;

            if (y == null)
                root = z;
            else if (z.Data < y.Data)
                y.Left = z;
            else
                y.Right = z;

            InsertFixup(z);
        }

        // Вспомогательный метод для поиска узла с минимальным значением в поддереве
        private Node Minimum(Node node)
        {
            while (node.Left != null)
                node = node.Left;

            return node;
        }

        // Вспомогательный метод для удаления узла
        private void DeleteFixup(Node x)
        {
            while (x != root && (x == null || x.Color == Color.Black))
            {
                if (x == x.Parent.Left)
                {
                    Node w = x.Parent.Right;

                    if (w.Color == Color.Red)
                    {
                        w.Color = Color.Black;
                        x.Parent.Color = Color.Red;
                        RotateLeft(x.Parent);
                        w = x.Parent.Right;
                    }

                    if ((w.Left == null || w.Left.Color == Color.Black) &&
                        (w.Right == null || w.Right.Color == Color.Black))
                    {
                        w.Color = Color.Red;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Right == null || w.Right.Color == Color.Black)
                        {
                            if (w.Left != null)
                                w.Left.Color = Color.Black;

                            w.Color = Color.Red;
                            RotateRight(w);
                            w = x.Parent.Right;
                        }

                        w.Color = x.Parent.Color;
                        x.Parent.Color = Color.Black;

                        if (w.Right != null)
                            w.Right.Color = Color.Black;

                        RotateLeft(x.Parent);
                        x = root;
                    }
                }
                else
                {
                    Node w = x.Parent.Left;

                    if (w.Color == Color.Red)
                    {
                        w.Color = Color.Black;
                        x.Parent.Color = Color.Red;
                        RotateRight(x.Parent);
                        w = x.Parent.Left;
                    }

                    if ((w.Right == null || w.Right.Color == Color.Black) &&
                        (w.Left == null || w.Left.Color == Color.Black))
                    {
                        w.Color = Color.Red;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Left == null || w.Left.Color == Color.Black)
                        {
                            if (w.Right != null)
                                w.Right.Color = Color.Black;

                            w.Color = Color.Red;
                            RotateLeft(w);
                            w = x.Parent.Left;
                        }

                        w.Color = x.Parent.Color;
                        x.Parent.Color = Color.Black;

                        if (w.Left != null)
                            w.Left.Color = Color.Black;

                        RotateRight(x.Parent);
                        x = root;
                    }
                }
            }

            if (x != null)
                x.Color = Color.Black;
        }

        // Основной метод для удаления узла из дерева
        public void Delete(int data)
        {
            Node z = root;

            while (z != null)
            {
                if (data == z.Data)
                {
                    Node y = z;
                    Color originalColor = y.Color;
                    Node x;

                    if (z.Left == null)
                    {
                        x = z.Right;
                        Transplant(z, z.Right);
                    }
                    else if (z.Right == null)
                    {
                        x = z.Left;
                        Transplant(z, z.Left);
                    }
                    else
                    {
                        y = Minimum(z.Right);
                        originalColor = y.Color;
                        x = y.Right;

                        if (y.Parent == z)
                        {
                            if (x != null)
                                x.Parent = y;
                        }
                        else
                        {
                            Transplant(y, y.Right);
                            y.Right = z.Right;

                            if (y.Right != null)
                                y.Right.Parent = y;
                        }

                        Transplant(z, y);
                        y.Left = z.Left;
                        y.Left.Parent = y;
                        y.Color = z.Color;
                    }

                    if (originalColor == Color.Black)
                        DeleteFixup(x);

                    break;
                }
                else if (data < z.Data)
                    z = z.Left;
                else
                    z = z.Right;
            }
        }

        // Вспомогательный метод для замены узла в дереве
        private void Transplant(Node u, Node v)
        {
            if (u.Parent == null)
                root = v;
            else if (u == u.Parent.Left)
                u.Parent.Left = v;
            else
                u.Parent.Right = v;

            if (v != null)
                v.Parent = u.Parent;
        }

        // Вспомогательный метод для рекурсивного вывода дерева в порядке возрастания
        private void InOrderTraversal(Node node)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left);
                Console.Write(node.Data + " ");
                InOrderTraversal(node.Right);
            }
        }

        // Метод для вывода дерева в порядке возрастания
        public void PrintInOrder()
        {
            InOrderTraversal(root);
            Console.WriteLine();
        }
        public void DrawTree()
        {
            DrawTree(root, "", true);
            Console.WriteLine();
        }

        // Вспомогательный метод для рекурсивной отрисовки дерева в консоли
        private void DrawTree(Node node, string indent, bool last)
        {
            if (node != null)
            {
                Console.Write(indent);

                if (last)
                {
                    Console.Write("└─");
                    indent += "  ";
                }
                else
                {
                    Console.Write("├─");
                    indent += "| ";
                }

                string nodeString = $"{node.Data} ({(node.Color == Color.Red ? "R" : "B")})";
                Console.WriteLine(nodeString);

                DrawTree(node.Left, indent, false);
                DrawTree(node.Right, indent, true);
            }
        }


        // Пример использования
        static void Main()
        {
            RedBlackTree tree = new();

            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(15);
            tree.Insert(5);

            Console.WriteLine("Отрисованное дерево: ");
            tree.DrawTree();

            Console.WriteLine("Дерево после вставки узлов:");
            tree.PrintInOrder();

            tree.Delete(15);

            Console.WriteLine("Дерево после удаления узла со значением 15:");
            tree.PrintInOrder();

            Console.WriteLine("Отрисованное дерево после вставки узла: ");
            tree.Insert(25);
            tree.DrawTree();
        }
    }

}