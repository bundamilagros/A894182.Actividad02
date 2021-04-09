using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A894182.Actividad02
{

    /*
    Una empresa de productos farmacéuticos necesita un sistema de control de stock. Para ello se le solicita una aplicación que permita:

o A) El ingreso de un catálogo de productos (identificado por número de producto), junto con el stock inicial para cada uno.

o B) El ingreso de una cantidad de pedidos y entregas de productos. Los pedidos restan al
    stock del producto y las entregas suman. El sistema debe ir controlando que las cantidades no sean menores a 0 en ningún momento.

o C) Al terminar, reporte el stock final de cada producto.
    */

    class Program
    {
        static void Main(string[] args)
        {
            Catalogo catalogo = new Catalogo();

            int opcion = MostrarMenu();
            Boolean cargarMenu = true;

            while (cargarMenu) {
                switch (opcion)
                {
                    case 1:  //cargar catalogo
                        catalogo.cargarProductos();
                        Boolean seguir = ValidarYN(Console.ReadLine());
                        while (seguir) {
                            catalogo.cargarProductos();
                            seguir = ValidarYN(Console.ReadLine().ToUpper());
                        }
                        if (!seguir) {
                            opcion = MostrarMenu();
                            break;
                        }
                        break;

                    case 2:  //pedido
                        Console.WriteLine("Ingrese el codigo del producto.\n");
                        int code = Validar(Console.ReadLine());
                        Console.WriteLine("Ingrese la cantidad solicitada.\n");
                        int cant = Validar(Console.ReadLine());
                        Pedido pedido = new Pedido();
                        Boolean okPedido = pedido.hacerPedido(code, cant, catalogo);
                        while (!okPedido) {
                            Console.WriteLine("Ingrese el codigo del producto.\n");
                            code = Validar(Console.ReadLine());
                            Console.WriteLine("Ingrese la cantidad solicitada.\n");
                            cant = Validar(Console.ReadLine());
                            okPedido = pedido.hacerPedido(code, cant, catalogo);
                        }
                        opcion = MostrarMenu();
                        break;
                    case 3:   //entrega

                        Console.WriteLine("Ingrese el codigo del producto.\n");
                        int codeE = Validar(Console.ReadLine());
                        Console.WriteLine("Ingrese la cantidad entregada.\n");
                        int cantE = Validar(Console.ReadLine());
                        Entrega entrega = new Entrega();
                        Boolean okEntrega = entrega.newEntrega(codeE, cantE, catalogo);
                        while (!okEntrega)
                        {
                            Boolean respuesta = ValidarYN(Console.ReadLine());
                            if (respuesta)
                            {
                                Console.WriteLine("Ingrese el nombre del nuevo producto.\n");
                                String nameP = Console.ReadLine();
                                Producto producto = new Producto();
                                producto.Codigo = codeE;
                                producto.Nombre = nameP;
                                producto.Stock = cantE;
                                catalogo.Total.Add(producto);
                                Console.WriteLine("Carga exitosa.\n");
                            }
                        }
                        opcion = MostrarMenu();
                        break;
                    case 4:  //stock final
                        foreach (Producto pr in catalogo.Total) {
                            Console.WriteLine("Producto: " + pr.Nombre + " (codigo " + pr.Codigo + "), cantidad en stock" + pr.Stock + ".\n");
                            opcion = MostrarMenu();
                        }
                        break;
                    case 5:
                        Console.WriteLine("Para salir, presione cualquier tecla. ¡Saludos!");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.\n");
                        opcion = Validar(Console.ReadLine());
                        break;
                } 
            }

            Console.ReadKey();
        }

        public static int Validar(String input) {
            Boolean opcionOk = int.TryParse(input, out int rtdo);
            while (!opcionOk || rtdo<0)
            {
                Console.WriteLine("La opcion no es valida. Intente de nuevo.\n");
                opcionOk = int.TryParse(Console.ReadLine(), out rtdo);
            }
            return rtdo;
        }

        public static int MostrarMenu() {

            Console.WriteLine("Bienvenido al sistema. \n Menú: \n");
            Console.WriteLine("1- Cargar catálogo.\n");
            Console.WriteLine("2- Ingresar un pedido.\n");
            Console.WriteLine("3- Ingresar una entrega.\n");
            Console.WriteLine("4- Reporte de stock final.\n");
            Console.WriteLine("5- Salir.\n");
            String input = Console.ReadLine();
            return Validar(input);
        }

        public static Boolean ValidarYN(String input) {
            input = input.ToUpper();
            Boolean seguir = false;

            while (!input.Equals("S") && !input.Equals("N")) {
                Console.WriteLine("La opcion no es valida. Intente de nuevo.\n");
                input = Console.ReadLine();
            }
            if (input.ToUpper().Equals("S")) {
                seguir= true;
            }
            if (input.ToUpper().Equals("N")) {
                seguir = false;
            }
            return seguir;
        }
    }
}

class Producto {
    private int codigo;
    private String nombre;
    private int stock;

    public int Codigo { get => codigo; set => codigo = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public int Stock { get => stock; set => stock = value; }
}

class Catalogo {
    private List<Producto> total = new List<Producto>();

    internal List<Producto> Total { get => total; set => total = value; }

    public void cargarProductos() {
        Producto p = new Producto();
        Console.WriteLine("Ingrese el codigo del producto: \n");
        int code = Validar(Console.ReadLine());
        Console.WriteLine("Ingrese el nombre del producto:\n");
        String name = Console.ReadLine();
        Console.WriteLine("Ingrese el la cantidad en stock del producto: \n");
        int stock = Validar(Console.ReadLine());
        p.Codigo = code;
        p.Nombre = name;
        p.Stock = stock;
        this.Total.Add(p);
        Console.WriteLine("¿Desea cargar otro producto?\n");
        Console.WriteLine("S -Si\n");
        Console.WriteLine("N -No\n");
    }
    public int Validar(String input)
    {
        Boolean opcionOk = int.TryParse(input, out int rtdo);
        while (!opcionOk || rtdo < 0)
        {
            Console.WriteLine("Error: Intente de nuevo.\n");
            opcionOk = int.TryParse(Console.ReadLine(), out rtdo);
        }
        return rtdo;
    }
}

class Pedido {
    private int codigo_producto;
    private int cantidad;


    public int Cantidad { get => cantidad; set => cantidad = value; }
    public int Codigo_producto { get => codigo_producto; set => codigo_producto = value; }

    public Boolean hacerPedido(int codigo_producto, int cantidad, Catalogo catalogo) {

        Producto producto = null;
        Boolean seguir = true;
        Pedido p = new Pedido();
        p.Cantidad = cantidad;
        p.Codigo_producto = codigo_producto;
       
        int contador = 0;

        foreach (Producto prod in catalogo.Total) {
            contador++;
            if (prod.Codigo == codigo_producto) {
                producto = prod;
                if (prod.Stock == 0 || prod.Stock - cantidad < 0) {
                    Console.WriteLine("ERROR: No se tiene stock del producto "+prod.Nombre);
                    producto = null;
                }
                break;
            }
        }

        if (contador == catalogo.Total.Count && producto==null)
        {
            Console.WriteLine("ERROR: No se ha encontrado el producto. Intente de nuevo.\n");
            seguir = false;
        }

        else {
         Console.WriteLine("Pedido confirmado: \n");
            Console.WriteLine("Producto: "+ producto.Nombre+ ", cantidad " + cantidad);
        }
        return seguir;
    }
}

class Entrega {
    private int codigo_producto;
    private int cantidad;

    public int Cantidad { get => cantidad; set => cantidad = value; }
    public int Codigo_producto { get => codigo_producto; set => codigo_producto = value; }

    public Boolean newEntrega(int codigo_producto, int cantidad, Catalogo catalogo)
    {
        Producto producto = null;
        Boolean seguir = true;
        Entrega e = new Entrega();
        e.Cantidad = cantidad;
        e.Codigo_producto = codigo_producto;

        int contador = 0;

        foreach (Producto prod in catalogo.Total)
        {
            contador++;
            if (prod.Codigo == codigo_producto)
            {
                producto = prod;
                break;
            }
        }
        if (contador == catalogo.Total.Count && producto == null)
        {
            Console.WriteLine("No se ha encontrado el producto, ¿Desea cargarlo en el catalogo?\n");
            seguir = false;
        }

        else
        {
            Console.WriteLine("Entrega efectuada: \n");
            Console.WriteLine("Producto: " + producto.Nombre + ", cantidad" + cantidad);
        }
        return seguir;
    }
}

