var Entities = new[]
{
    new Entities("greek", 7, 5, 10),
    new Entities("texas", 8, 9, 13),
    new Entities("european", 5, 10, 13)
};
var OrderItem = new[]
{
    new OrderItem("texas", "Medium", 1),
    new OrderItem("european", "Small", 2)
};

int costPizza = new Solution().solution(Entities, OrderItem);

Console.WriteLine($"La promoción de menor costo es: {costPizza}");

class Solution
{
    public int solution(Entities[] menu, OrderItem[] order)
    {
        var menuMap = menu.ToDictionary(p => p.name);

        int costoBase = CalcularCostoBase(order, menuMap);
        int costoD1 = CalcularCostoConDescuento1(order, menuMap, costoBase);
        int costoD2 = CalcularCostoConDescuento2(order, menuMap, costoBase);
        int costoD3 = CalcularCostoConDescuento3(order, menuMap);
        int costoD4 = CalcularCostoConDescuento4(order, menuMap, costoBase);

        int costoMinimo = costoBase;
        costoMinimo = Math.Min(costoMinimo, costoD1);
        costoMinimo = Math.Min(costoMinimo, costoD2);
        costoMinimo = Math.Min(costoMinimo, costoD3);
        costoMinimo = Math.Min(costoMinimo, costoD4);

        return costoMinimo;
    }

    private int GetPrice(Entities pizzaInfo, string size)
    {
        return size switch
        {
            "Small" => pizzaInfo.price_S,
            "Medium" => pizzaInfo.price_M,
            "Large" => pizzaInfo.price_L,
            _ => 0
        };
    }

    private int CalcularCostoBase(OrderItem[] order, Dictionary<string, Entities> menuMap)
    {
        return order.Sum(item => GetPrice(menuMap[item.name], item.size) * item.quantity);
    }

    private int CalcularCostoConDescuento1(OrderItem[] order, Dictionary<string, Entities> menuMap, int costoBase)
    {
        if (order.Sum(item => item.quantity) < 3)
        {
            return int.MaxValue;
        }

        var todosLosPrecios = order.SelectMany(item => Enumerable.Repeat(GetPrice(menuMap[item.name], item.size), item.quantity)).ToList();
        int ahorro = todosLosPrecios.Min();
        return costoBase - ahorro;
    }

    private int CalcularCostoConDescuento2(OrderItem[] order, Dictionary<string, Entities> menuMap, int costoBase)
    {
        var gruposParaDescuento = order
            .GroupBy(item => item.name)
            .Where(g => g.Sum(item => item.quantity) >= 5)
            .ToList();

        if (!gruposParaDescuento.Any())
        {
            return int.MaxValue;
        }

        int ahorroMaximo = 0;

        foreach (var grupo in gruposParaDescuento)
        {
            var preciosDelGrupo = grupo
                .SelectMany(item => Enumerable.Repeat(GetPrice(menuMap[item.name], item.size), item.quantity))
                .ToList();

            int costoDeLas5MasCaras = preciosDelGrupo.OrderByDescending(p => p).Take(5).Sum();
            int ahorroActual = costoDeLas5MasCaras - 100;

            if (ahorroActual > ahorroMaximo)
            {
                ahorroMaximo = ahorroActual;
            }
        }
        return costoBase - ahorroMaximo;
    }

    private int CalcularCostoConDescuento3(OrderItem[] order, Dictionary<string, Entities> menuMap)
    {
        int costoTotalConDescuento = 0;
        var ordenAgrupada = order.GroupBy(item => item.name);

        foreach (var grupo in ordenAgrupada)
        {
            var itemsDelGrupo = grupo.ToList();
            int cantidadLarge = itemsDelGrupo.FirstOrDefault(i => i.size == "Large")?.quantity ?? 0;
            int cantidadSmall = itemsDelGrupo.FirstOrDefault(i => i.size == "Small")?.quantity ?? 0;

            int smallsGratis = Math.Min(cantidadLarge, cantidadSmall);

            foreach (var item in itemsDelGrupo)
            {
                int precioUnitario = GetPrice(menuMap[item.name], item.size);
                costoTotalConDescuento += precioUnitario * item.quantity;
            }

            if (smallsGratis > 0)
            {
                int precioSmall = GetPrice(menuMap[grupo.Key], "Small");
                costoTotalConDescuento -= precioSmall * smallsGratis;
            }
        }

        return costoTotalConDescuento;
    }

    private int CalcularCostoConDescuento4(OrderItem[] order, Dictionary<string, Entities> menuMap, int costoBase)
    {
        var pizzasLarge = order
            .Where(item => item.size == "Large")
            .SelectMany(item => Enumerable.Repeat(item, item.quantity))
            .ToList();

        if (pizzasLarge.Count < 3)
        {
            return int.MaxValue;
        }

        var ahorros = pizzasLarge
            .Select(item => GetPrice(menuMap[item.name], "Large") - GetPrice(menuMap[item.name], "Medium"))
            .OrderByDescending(ahorro => ahorro)
            .ToList();

        int ahorroTotal = ahorros.Take(3).Sum();

        return costoBase - ahorroTotal;
    }
}