$.notify.addStyle('notify', {
    html: "<div><span data-notify-text /></div>",
    classes: {
        base: {
            "font-weight": "bold",
            "text-align":"center",
            "width": "350px",
            "height": "100px",
            "text-shadow": "0 1px 0 rgba(255, 255, 255, 0.5)",
            "background-color": "#fcf8e3",
            "border": "1px solid #fbeed5",
            "border-radius": "4px",
            "white-space": "normal",
            "display": "flex",
            "justify-content": "center",
            "align-items": "center",
            "background-repeat": "no-repeat",
            "background-position": "3px 7px"
        },
        error: {
            "color": "#B94A48",
            "background-color": "#F2DEDE",
            "border-color": "#EED3D7",
        },
        success: {
            "color": "#468847",
            "background-color": "#DFF0D8",
            "border-color": "#D6E9C6",

        },
        wait: {
            "color": "#191970",
            "background-color": "#00FFFF",
            "border-color": "#EED3D7",
        },
    }
});