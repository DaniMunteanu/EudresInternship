class Player
{
    constructor(x, y)
    {
        this.x = x;
        this.y = y;
        this.width = 50;
        this.height = 50;
        this.isDragged = false;
        this.yOffset = 0;
    }

    draw()
    {
        fill(255, 0, 0);
        strokeWeight(0);
        rect(this.x, this.y, this.width, this.height);
    }

    update()
    {
        if (this.isDragged)
        {
            this.y = floor(mouseY - this.yOffset);
        }
    }

    isReleased()
    {
        this.isDragged = false;
    }

    isCliked()
    {
        if (mouseX > this.x && mouseX < this.x + this.width && mouseY > this.y && mouseY < this.y + this.height)
        {
            this.isDragged = true;
            this.yOffset = mouseY - this.y;
        }
    }

    isTouchCup(cup)
    {
        if (this.x < cup.x + cup.width &&
            this.x + this.width > cup.x &&
            this.y < cup.y + cup.height &&
            this.y + this.height > cup.y)
        {
            return true;
        }
        return false;
    }
}

class Cup
{
    constructor(x, y)
    {
        this.x = x;
        this.y = y;
        this.width = 50;
        this.height = 50;
    }

    draw()
    {
        fill(0, 0, 255);
        strokeWeight(0);
        rect(this.x, this.y, this.width, this.height);
    }
}

function setup()
{
    createCanvas(1280, 720);
    player = new Player(width / 2 - 50, height / 2 - 50);
    cup = new Cup(width / 2 - 50, height - 200);
}

function draw()
{
    background(220);
    player.draw();
    player.update();
    cup.draw();

    if (player.isTouchCup(cup))
    {
        fill(0, 255, 0);
        rect(width / 2 + 200, height / 2 - 100, 200, 200);
    }
}

function mousePressed()
{
    player.isCliked();
}

function mouseReleased()
{
    player.isReleased();
}