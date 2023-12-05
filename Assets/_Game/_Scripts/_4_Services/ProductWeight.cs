using Assets._Game._Scripts._5_Managers;

[System.Serializable]
public class ProductWeight {
    public ProductType ProductType;
    public float Weight; // Массив весов для каждого уровня
    public ProductWeight(ProductType product, float weight) {
        ProductType = product;
        Weight = weight;
    }
}