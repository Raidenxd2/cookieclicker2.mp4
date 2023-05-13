using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

#if UNITY_ANDROID
public class IAPManager : ModIOBrowser.Implementation.Singleton<IAPManager>, IStoreListener
{
	private IStoreController controller;
    private IExtensionProvider extensions;
    public Game game;

    public IAPManager () 
    {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("com.raiden.cookieclicker2.mp4.cookies_50000", ProductType.Consumable, new IDs
        {
            {"com.raiden.cookieclicker2.mp4.cookies_50000", GooglePlay.Name},
            {"com.raiden.cookieclicker2.mp4.cookies_50000", MacAppStore.Name}
        });
        builder.AddProduct("com.raiden.cookieclicker2.mp4.cookies_100000", ProductType.Consumable, new IDs
        {
            {"com.raiden.cookieclicker2.mp4.cookies_100000", GooglePlay.Name},
            {"com.raiden.cookieclicker2.mp4.cookies_100000", MacAppStore.Name}
        });
        builder.AddProduct("com.raiden.cookieclicker2.mp4.starter_bundle", ProductType.Consumable, new IDs
        {
           {"com.raiden.cookieclicker2.mp4.starter_bundle", GooglePlay.Name},
           {"com.raiden.cookieclicker2.mp4.starter_bundle", MacAppStore.Name} 
        });

        UnityPurchasing.Initialize (this, builder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed (InitializationFailureReason error)
    {

    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    {
        if (e.purchasedProduct.definition.id == "com.raiden.cookieclicker2.mp4.cookies_50000")
        {
            game.Cookies += 50000;
        }
        if (e.purchasedProduct.definition.id == "com.raiden.cookieclicker2.mp4.cookies_100000")
        {
            game.Cookies += 100000;
        }
        if (e.purchasedProduct.definition.id == "com.raiden.cookieclicker2.mp4.starter_bundle")
        {
            var oldCookies = game.Cookies;
            game.Cookies = 9999999999999;
            game.StarterBundleBought = true;
            for (int i = 0; i < 10; i++)
            {
                game.BuyGrandma();
            }
            for (int i = 0; i < 10; i++)
            {
                game.BuyDoublecookie();
            }
            for (int i = 0; i < 10; i++)
            {
                game.BuyAutoclicker();
            }
            for (int i = 0; i < 5; i++)
            {
                game.BuyDrill();
            }
            game.Cookies = oldCookies += 150000;
            game.HideStarterBundleButton();
        }
        return PurchaseProcessingResult.Complete;
        
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
    {
    }

    public void OnPurchaseClicked(string productId) 
    {
        controller.InitiatePurchase(productId);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
#endif