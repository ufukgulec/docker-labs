#!/usr/bin/env bash

echo "=========================================="
echo "   🧹 Docker Cleanup Tool / Temizlik     "
echo "=========================================="
echo ""

# 1. Mevcut Docker Disk Kullanımını Göster
echo "📊 Mevcut Disk Kullanımı:"
docker system df
echo ""

read -p "❓ Kullanılmayan tüm container, image, network ve dangling volume'ları silmek istiyor musunuz? (y/N): " confirm

if [[ "$confirm" =~ ^[Yy]$ ]]; then
    echo ""
    echo "🗑️ Temizlik başlatılıyor..."
    
    # Durdurulmuş konteynerları, kullanılmayan ağları ve imajları siler
    docker system prune -a --volumes -f

    echo ""
    echo "✅ Temizlik tamamlandı! Güncel Disk Kullanımı:"
    docker system df
else
    echo "❌ Temizlik iptal edildi."
fi