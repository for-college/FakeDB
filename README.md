# FakeDB

### Структура
#### FakeDatabase класс для работы с "БД"
- Добавление юзера
- Удаление юзера
- Получение юзеров
- Валидация
- Выход

#### UserMenu класс для отображения данных
По сути является своеобразным View, прослойка до самого класса БД.
- Display - отображение интерфейса
- AddUser - добавление юзера
- RemoveUser - удаление юзера
- DisplayAllUsers - отображение всех юзеров

#### User класс с полями для юзера
