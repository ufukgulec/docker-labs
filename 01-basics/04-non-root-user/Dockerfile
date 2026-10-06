FROM node:20-alpine

WORKDIR /app

COPY server.js .

# Güvenlik Adımı: Root yetkisini bırakıp imajda var olan 'node' kullanıcısına geçiyoruz
USER node

EXPOSE 3000

CMD ["node", "server.js"]