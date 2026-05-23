# CatalogoApp - Tu Catálogo Musical

¡Hola! Bienvenidos a **CatalogoApp**. Esta es una plataforma web desarrollada por **epinki07** donde puedes descubrir música, guardar tus álbumes favoritos, darles una calificación y compartir lo que piensas con otros usuarios de forma súper sencilla.

##  ¿Qué puedes hacer en la app?

-  **Explorar música:** Navega por un catálogo lleno de álbumes y canciones con todos sus detalles.
-  **Calificar:** Dales de 1 a 5 estrellas a los álbumes que escuches. El promedio se calcula solito.
-  **Opinar:** Deja tus reseñas y lee las de los demás.
-  **Tu propio espacio:** Crea tu cuenta e inicia sesión de forma segura para guardar tu perfil.
- **Búsqueda rápida:** Encuentra justo lo que quieres escuchar usando nuestros filtros.

---

## ¿Cómo lo construimos?

Para hacer esto posible, dividimos el trabajo usando herramientas modernas pero tratando de mantenerlo simple:

- **El motor (Backend):** Usamos ASP.NET Core 10 (C#). Todo está ordenado usando el modelo MVC (para separar las pantallas de la lógica).
- **Lo que ves (Frontend):** HTML, CSS (diseñado para que se vea bien tanto en tu compu como en tu celular) y JavaScript para que la página responda rápido.
- **Los datos:** En lugar de una base de datos pesada, guardamos todo en archivos `.json`. Es súper ligero y rápido de leer usando `System.Text.Json`.

---
##  Uso de Inteligencia Artificial

Para este proyecto, utilizamos IA como una herramienta de apoyo para aprender y optimizar nuestro flujo de trabajo. Principalmente nos ayudó a entender mejor cómo programar un sistema de inicio y cierre de sesión (login/logout) que fuera seguro y tuviera sentido. También tomamos algunas de sus sugerencias para mejorar el diseño de la interfaz y hacer que la experiencia de uso sea más fluida. 

Finalmente, utilizamos IA para limpiar y reparar los errores de conflictos de Git que se habían generado en este archivo README, unificando la información para dejarlo mucho más ordenado y fácil de leer.
## 📁 ¿Cómo está organizado el código?

Dividimos el proyecto en varias carpetas para no revolver las cosas y que sea más fácil trabajar en equipo:

```text
CatalogoApp/
├── Catalogo.Presentation/      # Lo visual: Aquí están las pantallas y los controladores.
│   ├── Controllers/            
│   ├── Views/                  # Las pantallas (Inicio, Catálogo, Login, etc.)
│   ├── wwwroot/                # Estilos (CSS), imágenes y scripts (JS)
│   └── data/                   # Aquí viven nuestros archivos .json con la info
│
├── Catalogo.Application/       # El cerebro: Los servicios que hacen que todo funcione.
│
├── Catalogo.Infrastructure/    # Los archivos que se encargan de leer y escribir en los JSON.
│
└── Catalogo.Domain/            # Las plantillas de nuestra información (Usuarios, Comentarios, etc).
