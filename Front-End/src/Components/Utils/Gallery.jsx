import { IMGES_URL } from "../../Api/Api";

export default function Gallery({ images = [] }) {
  return (
    <>
      <style>{`
                @import url('https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap');
            
                * {
                    font-family: 'Poppins', sans-serif;
                }
            `}</style>
      <div className="flex items-center gap-2 h-[300px] w-full max-w-4xl mt-10 mx-auto">
        {images.map((img) => (
          <div className="relative group flex-grow transition-all w-56 rounded-lg overflow-hidden h-[300px] duration-500 hover:w-full">
            <img
              className="h-full w-full object-cover object-center"
              src={`${IMGES_URL}${img.imageUrl !== undefined ? img.imageUrl : img}`}
              alt="image"
            />
          </div>
        ))}
      </div>
    </>
  );
}
