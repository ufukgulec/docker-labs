import os
from http.server import HTTPServer, BaseHTTPRequestHandler

PORT = int(os.environ.get("PORT", 8000))
APP_NAME = os.environ.get("APP_NAME", "Docker Lab App")

class SimpleHandler(BaseHTTPRequestHandler):
    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-type", "text/plain; charset=utf-8")
        self.end_headers()
        response = f"Merhaba! Uygulama: {APP_NAME}\n"
        self.wfile.write(response.encode("utf-8"))

if __name__ == "__main__":
    server = HTTPServer(("0.0.0.0", PORT), SimpleHandler)
    print(f"Sunucu {PORT} portunda başlatıldı...")
    server.serve_forever()